# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

C# client library implementing the **ROCO/Fleischmann Z21 LAN protocol** (V1.13) over UDP. Platform-independent, event-driven, SOLID. The protocol spec PDFs are linked in `README.md`; the command/response support matrix (✅/❌) also lives there and should be kept in sync when commands or handlers are added.

## Build, Test, Run

All commands operate on the solution at `src/Z21.sln`. The library targets `net8.0;net8.0-windows` and only builds for the `x64` platform.

```bash
dotnet restore src/Z21.sln
dotnet build   src/Z21.sln
dotnet test    src/Z21.sln                       # all tests
dotnet test    src/Z21.sln --filter "FullyQualifiedName~SetLocoDriveCommandTest"   # single test class
dotnet run     --project src/Z21.Console          # demo console app against a live/simulated Z21
```

Mutation testing (Stryker.NET) is run from inside the test project and gates CI:

```bash
dotnet tool install -g dotnet-stryker
cd src/Z21.Client.UnitTest
dotnet stryker --reporter html --reporter progress --mutation-level Complete --threshold-high 98 --threshold-low 90 --break-at 85
```

These thresholds (and `coverage-analysis: perTest`) are pinned in `stryker-config.json` in each test
project, so a bare `dotnet stryker` uses them. Target line coverage is ~95%. `Z21.Client` (all protocol
logic) holds **break 85** and currently scores ~85%. `CommandStation.Transport.Udp` uses a lower
**break 60** (reports 90/98): its residual mutants are non-observable socket internals (`UdpClient` is
sealed/non-mockable — resource disposal, `AllowNatTraversal`, `GC.SuppressFinalize`, equivalent logical
ops), so ~66% is the accepted floor for that thin transport shell.

Tests use **NUnit + Moq**. New protocol logic is expected to be both unit-tested and to survive mutation testing — the bar is high, so assert on exact datagram bytes.

> **Architecture is mid-refactor to v7** (decoupling transport + protocol so other command
> stations can be added). The layering below is the current state. The neutral root namespace
> `CommandStation` is provisional (brand name TBD). See the plan and the `architecture-refactor-v7`
> memory for context.

## Projects

Two orthogonal axes are separated into assemblies: **transport** (how bytes move) and **protocol**
(the Z21 wire format). The abstractions assembly is protocol- and transport-neutral.

- **CommandStation.Abstractions** (ns `CommandStation`) — neutral contracts, no Z21/UDP specifics:
  `ITransport`, `IFrameReader` (+ event args) under `Transport`/`Framing`; the domain API
  `ICommandStation` + capability interfaces (`ILocoControl`, `IAccessoryControl`,
  `ITrackPowerControl`, `ISystemInfoProvider`, `IProgrammingControl`, `IFeedbackControl`,
  `IFastClockControl`); and the domain vocabulary under `Model`
  (enums like `DccSpeedMode`/`DrivingDirection`, data like `LocoInfoData`/`SystemState`/`FirmwareVersion`/
  `CvValue`/`FeedbackData`/`ModelTime`). Z21-only protocol features (LocoNet raw tunneling, CAN, RailCom,
  zLink booster/decoder/adapter) have **no neutral capability**; reach them via the Z21 escape hatch
  (`IZ21CommandStation.Commands` + the Z21 response-handler events).
- **CommandStation.Transport.Udp** — `UdpTransport : ITransport` + `UdpTransportOptions`. A future
  serial transport would be a sibling assembly; nothing in `Z21.Client` references it directly.
- **Z21.Client** (NuGet id `Z21`, root ns `Z21`) — the Z21 protocol implementation: commands +
  `IZ21CommandFactory`, response handlers/parsers, `Z21FrameReader`/`Z21FrameBuilder`, the
  `IAddressCodec`/`ILocoSpeedCodec` codecs, and `Z21CommandStation : IZ21CommandStation`. References
  only `CommandStation.Abstractions`. A `global using CommandStation.Model;` makes the domain
  vocabulary available without per-file usings.
- **Z21.DependencyInjection** / **Z21.Autofac** — `AddZ21(...)` extensions; reference
  `CommandStation.Transport.Udp` to wire the concrete UDP transport.
- **Z21.Console** — runnable demo / manual test harness.
- **\*.UnitTest(s)** — one test project per shippable project.

## Architecture

Four layers, bottom → top — transport and protocol are independent:

- **Transport** (`ITransport`) — a raw byte pipe (`ConnectAsync`/`DisconnectAsync`/`SendAsync`,
  `OnBytesReceived`, `OnConnectionChanged`). `UdpTransport` today; serial later.
- **Framing** (`IFrameReader`) — reassembles the byte stream into discrete frames. `Z21FrameReader`
  buffers partial frames using the `DataLen` length-prefix, so it is correct for both message
  (UDP) and stream (serial) transports.
- **Protocol** — Z21 encode/decode. `IZ21FrameBuilder` (+ `IAddressCodec`/`ILocoSpeedCodec`) builds
  command bytes (`BuildXBus`/`BuildLan`); handlers (`IZ21ResponseHandler`, `CanHandle`/`Handle`) and
  parsers (`IZ21ResponseParser`) decode frames and raise typed `On...Received` events. Commands and
  these services are all **injected**, never static.
- **Domain** (`ICommandStation` + capabilities) — the protocol-agnostic public API. `Z21CommandStation`
  implements it (+ a Z21 raw escape hatch `IZ21CommandStation` exposing `Commands` and
  `SendCommandsAsync(params IZ21Command[])`).

Data flow:

1. `ICommandStation` op (e.g. `DriveAsync`) → `IZ21CommandFactory` builds an `IZ21Command` (bytes via
   `IZ21FrameBuilder` + codecs) → `SendCommandsAsync` concatenates command `Data` into **one packet**
   (so simultaneous actions like double-traction stay atomic), enforces `MaxUdpPayload` (1472), and
   sends via `ITransport`. A `DelayedAction` keep-alive re-sends a firmware query after
   `Z21Options.KeepAliveInterval` (default 45s).
2. `ITransport.OnBytesReceived` → `Z21FrameReader.Append` → `OnFrameReceived` per complete frame.
3. The dispatcher `Z21ResponseHandler` (distinct from individual handlers) offers each frame to every
   `IZ21ResponseHandler` whose `CanHandle` returns true; handler exceptions are caught and logged.
4. Handlers raise typed events; `Z21CommandStation` re-raises them as neutral capability events
   (`LocoInfoReceived`, `SystemStateReceived`, `TrackPowerChanged`, …).

`Z21CommandStation.ConnectAsync` connects the transport then runs `LogOnAsync` (broadcast flags +
firmware query). There is **no ICMP watchdog** — liveness is the transport connection state plus the
protocol keep-alive (the old `Z21Watchdog` was removed as part of the transport decoupling).

The dispatcher must be instantiated for inbound handling to work — both DI extensions register it as
an **activated/auto-activated singleton** so it wires up `ITransport.OnBytesReceived` eagerly.

### DI registration

Both `Z21DependencyInjectionExtension` and `Z21AutofacExtensions` discover all `IZ21ResponseHandler` /
`IZ21ResponseParser` implementations by reflection and register each concrete type plus all of its
handler/parser interfaces as singletons. **Adding a new handler or parser requires no registration
changes** — implement the interface and it is picked up automatically. `AddZ21(...)` takes optional
`Action<UdpTransportOptions>` and `Action<Z21Options>` configurators. Both containers must stay
behavior-equivalent.

### Conventions

- **Coding rules are strict** (see the `coding-rules` memory): no static methods/properties except
  `const` fields; no empty catch blocks; a new subtype must require zero edits outside its own file;
  TDD test-first with a quotable red run. Committing is allowed, but **never include AI
  attribution in anything that touches git or GitHub** — not in commit messages, PR titles or
  descriptions, issue/PR comments, tags, or release notes. Concretely: no `Co-Authored-By`
  trailer, no "Generated with Claude Code" (or any similar "made/assisted by AI") line, and no
  AI tool name anywhere in the history or on GitHub. This applies to every `git` and `gh`/GitHub
  API action without exception.
- The library assumes a **little-endian** host (`Z21CommandStation` throws
  `PlatformNotSupportedException` otherwise); protocol multi-byte fields are little-endian.
- Command construction goes through `IZ21CommandFactory` (the station exposes it as `Commands`); a new
  command is one new file plus an optional factory method.
- Custom exceptions live in `Core/Exception/`; `MtuPayloadLengthExceededException.ThrowIfExceeded`
  guards payload size against `Z21CommandStation.MaxUdpPayload`.
- Logging is via `ILogger<T>?` (Microsoft.Extensions.Logging.Abstractions), always optional.

## Versioning & CI

- **Versioning — Nerdbank.GitVersioning (nbgv).** `version.json` at the repo root holds
  the base version (`"version": "7.0"`) and `publicReleaseRefs` (`^refs/heads/main$`). nbgv
  is referenced once in the root `Directory.Build.props` (`PrivateAssets="all"`), so it
  stamps the assembly **and** NuGet package versions of every project automatically from
  the base version + **git height** — the patch increments on every commit (`7.0.<height>`
  on `main`; off-`main` builds get a `-g<commit>` prerelease suffix). There is **no**
  `-p:Version` in CI and no hand-bumping; bump `"version"` in `version.json` for the next
  minor/major. **Consequence:** nbgv fails on shallow clones, so every workflow that builds
  (`Build.yml`, `Release.yml`, `MutationTesting.yml`) checks out with `fetch-depth: 0`.
- **CI — `.github/workflows/Build.yml`:** builds (`Release`) and tests on every push and
  pull request. It does **not** pack or publish.
- **CD — `.github/workflows/Release.yml`:** publishes on every **push to `main`** (i.e.
  when a PR is merged) and on manual `workflow_dispatch`. It builds → tests → packs the
  whole solution **once** (`dotnet pack src/Z21.sln`, producing all five packable packages
  — `Z21`, `Z21.DependencyInjection`, `Z21.Autofac`, `CommandStation.Abstractions`,
  `CommandStation.Transport.Udp`; Console + test projects are `IsPackable=false`) → pushes
  with `--skip-duplicate`. The push step is guarded `if: github.ref == 'refs/heads/main'`,
  so a manual dispatch from a non-`main` branch is a pack-only dry run.
- The `GeneratePackageOnBuild=true` properties in the five packable csproj are redundant
  with the explicit Release pack and may be removed later.
