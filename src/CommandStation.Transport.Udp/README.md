# CommandStation.Transport.Udp

A UDP implementation of `ITransport` from
[CommandStation.Abstractions](https://www.nuget.org/packages/CommandStation.Abstractions).
It's the raw byte pipe the command-station libraries send and receive frames over.

Most people never use this directly — the [Z21](https://www.nuget.org/packages/Z21) DI
packages wire it up for you. Pull it in on its own only if you're composing the transport
by hand or building another command station on top of the shared abstractions.

See the [project page](https://github.com/jaak0b/Z21). Licensed under GPL-3.0.
