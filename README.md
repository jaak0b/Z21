# Z21 [![Build](https://github.com/jaak0b/Z21/actions/workflows/Build.yml/badge.svg)](https://github.com/jaak0b/Z21/actions/workflows/Build.yml) [![Github Pages](https://github.com/jaak0b/Z21/actions/workflows/BuildAndDeployDoc.yml/badge.svg)](https://github.com/jaak0b/Z21/actions/workflows/BuildAndDeployDoc.yml) [![Mutation Testing](https://github.com/jaak0b/Z21/actions/workflows/MutationTesting.yml/badge.svg)](https://github.com/jaak0b/Z21/actions/workflows/MutationTesting.yml)

<img src="https://github.com/ZIMO-Elektronik/Z21/raw/master/data/images/logo.png" width="15%" align="right">


The ROCO Z21 is a command station with support for LocoNet, R-Bus and XpressNet devices. It has an open LAN interface with a well-documented protocol that has been continuously developed since then. This C# library of the same name contains platform-independent code for the client-side (i.e. the part that runs on a computer/smart phone) implementation of the protocol.

## Protocol
The official documentation of the protocol can be downloaded from the ROCO homepage in English and German.

| English                                                                                                                                                                               | German                                                                                                                                                       |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| [Z21 LAN protocol V1.13](https://www.z21.eu/media/Kwc_Basic_DownloadTag_Component/root-en-main_47-1652-959-downloadTag-download/default/d559b9cf/1628743384/z21-lan-protokoll-en.pdf) | [Z21 LAN Protokoll V1.13](https://www.z21.eu/media/Kwc_Basic_DownloadTag_Component/47-1652-959-downloadTag/default/69bad87e/1699290251/z21-lan-protokoll.pdf)|


## Features

- Platform-independent
- Lightweight 
- SOLID and event-driven architecture - Allowing you to expand the library to your liking.
- Fully unit and mutation tested
- Supports dependency injection out of the box via Z21.DependencyInjection.
- Fine grained command/response handler interfaces with great documentation. 
- Right now the following features of the Z21 LAN protocoll are implemented. See below for a fully detailed list of all supported commands/responses.
    - System ✅
    - Driving ✅
    - Switching ✅
    - CV / POM programming ✅
    - R-BUS feedback ✅
    - RailCom ✅
    - LocoNet gateway ✅
    - CAN (detector & booster) ✅
    - Fast clock (model time) ✅
    - zLink booster / decoder / adapter ✅
 
## Documentation

The API reference, generated from the library's source XML documentation comments, plus a
short getting-started guide, is published at **[jaak0b.github.io/Z21](https://jaak0b.github.io/Z21/)**.
It is built with [DocFX](https://dotnet.github.io/docfx/) from the config in [`docfx/`](docfx);
to preview locally run `dotnet tool install -g docfx` then `docfx docfx/docfx.json --serve`.

## Getting Started
Get started by downloading the provided [Z21](https://www.nuget.org/packages/Z21/) nuget package.

### Using the command station
The headline API is the protocol-agnostic `ICommandStation` (implemented for Z21 by `Z21CommandStation`).
Resolve it from your container, connect, then drive locomotives, switch turnouts, control track power and
subscribe to status events through its capability interfaces (`ILocoControl`, `IAccessoryControl`,
`ITrackPowerControl`, `ISystemInfoProvider`).

```csharp
    var station = container.Resolve<IZ21CommandStation>(); // or ICommandStation
    await station.ConnectAsync();

    station.LocoInfoReceived += (_, loco) => Console.WriteLine($"Loco {loco.LocoAddress} @ {loco.LocoSpeed}");

    await station.DriveAsync(locoAddress: 13, DccSpeedMode.Steps128, DrivingDirection.Forward, speed: 40);
    await station.TrackPowerOffAsync();
```

#### Raw commands
For full control you can still build and send raw Z21 commands. Build them via `station.Commands`
(an `IZ21CommandFactory`) and hand them to `SendCommandsAsync`.
Multiple commands are sent in the same UDP packet — important when actions must happen simultaneously
(e.g. a double-traction where both locomotives must change speed at once).

> [!WARNING]
> When sending multiple commands at once take note of the maximum payload length. If the commands exceed that length an exception will be thrown.

```csharp
    await station.SendCommandsAsync(station.Commands.Create<GetFirmwareVersionCommand>()); // single command
    await station.SendCommandsAsync(station.Commands.Create<GetFirmwareVersionCommand>(), station.Commands.Create<GetLocoInfoCommand>((ushort)13)); // one UDP packet
```

###

### Autofac
> [!IMPORTANT]
> While this library works without Autofac, Autofac is still recommend as it makes usage of this library much easier.
[Z21.Autofac](https://www.nuget.org/packages/Z21.Autofac/) provides extension methods to register all required classes directly in the container.

```csharp
    var builder = new ContainerBuilder();
    builder.AddZ21(transport => transport.RemoteEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.111"), 21105));
    var container = builder.Build();
    var station = container.Resolve<IZ21CommandStation>();
```

`AddZ21` optionally takes an `Action<UdpTransportOptions>` (transport/endpoint settings) and an
`Action<Z21Options>` (protocol settings such as broadcast flags and keep-alive interval).

### Dependency Injection
Dependency Injection is supported natively via [Z21.DependencyInjection](https://www.nuget.org/packages/Z21.DependencyInjection/) and requires the use of hosted services.
Z21 registers background components that must run inside the .NET Generic Host lifecycle.

```csharp
    var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddZ21();
    })
    .Build();

    await host.RunAsync();
```
The host is responsible for starting all Z21‑related hosted services and managing their lifetime.
    
## Z21 Commands

> [!NOTE]
> ✅ - Implemented and ready to use. ❌ - Not yet implemented. 

| Command| Status | |
| ------------- | ------------- | ------------- |
| LAN_GET_SERIAL_NUMBER | ✅ | |
| LAN_GET_CODE | ✅ | |
| LAN_GET_HWINFO | ✅ | |
| LAN_LOGOFF | ✅ | |
| LAN_X_GET_VERSION | ✅ | |
| LAN_X_GET_STATUS | ✅ | |
| LAN_X_SET_TRACK_POWER_OFF | ✅ | |
| LAN_X_SET_TRACK_POWER_ON | ✅ | |
| LAN_X_DCC_READ_REGISTER | ✅ | |
| LAN_X_CV_READ | ✅ | |
| LAN_X_DCC_WRITE_REGISTER | ✅ | |
| LAN_X_CV_WRITE | ✅ | |
| LAN_X_MM_WRITE_BYTE | ✅ | |
| LAN_X_GET_TURNOUT_INFO | ✅ | |
| LAN_X_GET_EXT_ACCESSORY_INFO | ✅ | |
| LAN_X_SET_TURNOUT | ✅ | |
| LAN_X_SET_EXT_ACCESSORY | ✅ | |
| LAN_X_SET_STOP | ✅ | |
| LAN_X_SET_LOCO_E_STOP | ✅ | |
| LAN_X_PURGE_LOCO | ✅ | |
| LAN_X_GET_LOCO_INFO | ✅ | |
| LAN_X_SET_LOCO_DRIVE | ✅ | |
| LAN_X_SET_LOCO_FUNCTION | ✅ | |
| LAN_X_SET_LOCO_FUNCTION_GROUP | ✅ | |
| LAN_X_SET_LOCO_BINARY_STATE | ✅ | |
| LAN_X_CV_POM_WRITE_BYTE | ✅ | |
| LAN_X_CV_POM_WRITE_BIT | ✅ | |
| LAN_X_CV_POM_READ_BYTE | ✅ | |
| LAN_X_CV_POM_ACCESSORY_WRITE_BYTE | ✅ | |
| LAN_X_CV_POM_ACCESSORY_WRITE_BIT | ✅ | |
| LAN_X_CV_POM_ACCESSORY_READ_BYTE | ✅ | |
| LAN_X_GET_FIRMWARE_VERSION | ✅ | |
| LAN_SET_BROADCASTFLAGS | ✅ | |
| LAN_GET_BROADCASTFLAGS | ✅ | |
| LAN_GET_LOCOMODE | ✅ | |
| LAN_SET_LOCOMODE | ✅ | |
| LAN_GET_TURNOUTMODE | ✅ | |
| LAN_SET_TURNOUTMODE | ✅ | |
| LAN_RMBUS_GETDATA | ✅ | |
| LAN_RMBUS_PROGRAMMODULE | ✅ | |
| LAN_SYSTEMSTATE_GETDATA | ✅ | |
| LAN_RAILCOM_GETDATA | ✅ | |
| LAN_LOCONET_FROM_LAN | ✅ | |
| LAN_LOCONET_DISPATCH_ADDR | ✅ | |
| LAN_LOCONET_DETECTOR | ✅ | |
| LAN_CAN_DETECTOR | ✅ | |
| LAN_CAN_DEVICE_GET_DESCRIPTION | ✅ | |
| LAN_CAN_DEVICE_SET_DESCRIPTION | ✅ | |
| LAN_CAN_BOOSTER_SET_TRACKPOWER | ✅ | |
| LAN_FAST_CLOCK_CONTROL | ✅ | |
| LAN_FAST_CLOCK_SETTINGS_GET | ✅ | |
| LAN_FAST_CLOCK_SETTINGS_SET | ✅ | |
| LAN_BOOSTER_SET_POWER |✅ | |
| LAN_BOOSTER_GET_DESCRIPTION | ✅ | |
| LAN_BOOSTER_SET_DESCRIPTION | ✅ | |
| LAN_BOOSTER_SYSTEMSTATE_GETDATA | ✅ | |
| LAN_DECODER_GET_DESCRIPTION | ✅ | |
| LAN_DECODER_SET_DESCRIPTION | ✅ | |
| LAN_DECODER_SYSTEMSTATE_GETDATA | ✅ | |
| LAN_ZLINK_GET_HWINFO| ✅ | |

## Z21 Responses

> [!NOTE]
> ✅ - Implemented and ready to use. ❌ - Not yet implemented. 

| Response | Status | |
| ------------- | ------------- | ------------- |
 | LAN_GET_SERIAL_NUMBER | ✅ | |
 | LAN_GET_CODE | ✅ | |
 | LAN_GET_HWINFO  | ✅ | |
 | LAN_X_TURNOUT_INFO  | ✅ | |
 | LAN_X_EXT_ACCESSORY_INFO  | ✅ | |
 | LAN_X_BC_TRACK_POWER_OFF | ✅ | |
 | LAN_X_BC_TRACK_POWER_ON  | ✅ | |
 | LAN_X_BC_PROGRAMMING_MODE  | ✅ | |
 | LAN_X_BC_TRACK_SHORT_CIRCUIT | ✅ | |
 | LAN_X_CV_NACK_SC  | ✅ | |
 | LAN_X_CV_NACK | ✅ | |
 | LAN_X_UNKNOWN_COMMAND  | ✅ | |
 | LAN_X_STATUS_CHANGED  | ✅ | |
 | LAN_X_GET_VERSION  | ✅ | |
 | LAN_X_CV_RESULT  | ✅ | |
 | LAN_X_BC_STOPPED  | ✅ | |
 | LAN_X_LOCO_INFO  | ✅ | |
 | LAN_X_GET_FIRMWARE_VERSION | ✅ | |
 | LAN_GET_BROADCASTFLAGS  | ✅ | |
 | LAN_GET_LOCOMODE | ✅ | |
 | LAN_GET_TURNOUTMODE  | ✅ | |
 | LAN_RMBUS_DATACHANGED  | ✅ | |
 | LAN_SYSTEMSTATE_DATACHANGED | ✅ | |
 | LAN_RAILCOM_DATACHANGED  | ✅ | |
 | LAN_LOCONET_Z21_RX  | ✅ | |
 | LAN_LOCONET_Z21_TX  | ✅ | |
 | LAN_LOCONET_FROM_LAN  | ✅ | |
 | LAN_LOCONET_DISPATCH_ADDR | ✅ | |
 | LAN_LOCONET_DETECTOR  | ✅ | |
 | LAN_CAN_DETECTOR  | ✅ | |
 | LAN_CAN_DEVICE_GET_DESCRIPTION | ✅ | |
 | LAN_CAN_BOOSTER_SYSTEMSTATE_CHGD | ✅ | |
 | LAN_FAST_CLOCK_DATA  | ✅ | |
 | LAN_FAST_CLOCK_SETTINGS_GET  | ✅ | |
 | LAN_BOOSTER_GET_DESCRIPTION  | ✅ | |
 | LAN_BOOSTER_SYSTEMSTATE_DATACHANGED  | ✅ | |
 | LAN_DECODER_GET_DESCRIPTION  | ✅ | |
 | LAN_DECODER_SYSTEMSTATE_DATACHANGED  | ✅ | |
 | LAN_ZLINK_GET_HWINFO  | ✅ | |
 
## Contributing

Contributions are welcome! If you encounter any issues or have suggestions, please open an issue or submit a pull request.

## License

This project is licensed under the GPL-3.0 license - see the LICENSE file for details.
