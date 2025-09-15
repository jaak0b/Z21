# Z21 [![.NET](https://github.com/Jakob-Eichberger/Z21/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Jakob-Eichberger/Z21/actions/workflows/dotnet.yml) [![Github Pages](https://github.com/Jakob-Eichberger/Z21/actions/workflows/BuildAndDeployDoc.yml/badge.svg)](https://github.com/Jakob-Eichberger/Z21/actions/workflows/BuildAndDeployDoc.yml) [![Mutation Testing](https://github.com/Jakob-Eichberger/Z21/actions/workflows/MutationTesting.yml/badge.svg)](https://github.com/Jakob-Eichberger/Z21/actions/workflows/MutationTesting.yml)

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
- Fully unit tested
- Supports dependency injection out of the box via Z21.DependencyInjection.
- Fine grained command/response handler interfaces allowing for simple use. See below for a full list of all supported commands/responses.
    - System ✅
    - Driving ✅
    - Switching ✅
 
## Getting Started
Get started by downloading the provided nuget Z21 package. To register the Z21 client use the Z21.DependencyInjection nuget package.

### Commands
All Commands can be found in the Z21.Core.Command namespace. 
#### Sending Commands
> [!WARNING]
> When sending multiple commands at once take note of the maximum payload length. If the commands exceeds that length an exception will be thrown. 

Create a command instance and hand it to the Z21Client.SendCommandsAsync method. 
Multiple commands can be send at the same time in the same UDP packet. 
This is important if certain actions should happen at the same time (i.e. controlling locos in a double traction where it is critical that both locomotives change speed at the same time)

```csharp
    await Z21Client.SendCommandsAsync(new GetFirmwareVersionCommand()); // Sends a single command
    await Z21Client.SendCommandsAsync(new GetFirmwareVersionCommand(), new GetLocoInfoCommand(locoAddress: 13); // Send multiple commands in a single UDP packet
```

###

### Dependency Injection
> [!IMPORTANT]
> While this library works without dependency injection, DI is still recommend as it makes usage of this library much easier.
Z21.DependencyInjection provides extension methods to register all required classes directly in the container.

```csharp
    services.ConfigureZ21Client(Z21Configuration.Defaults.IpEndPoint);
    services.AddZ21Client();
    services.AddZ21Transport();
    services.AddZ21ResponseParser();
    services.AddZ21ResponseHandler();
```
    
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
| LAN_X_DCC_READ_REGISTER | ❌ | |
| LAN_X_CV_READ | ✅ | |
| LAN_X_DCC_WRITE_REGISTER | ❌ | |
| LAN_X_CV_WRITE | ❌ | |
| LAN_X_MM_WRITE_BYTE | ❌ | |
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
| LAN_X_SET_LOCO_FUNCTION_GROUP | ❌ | |
| LAN_X_SET_LOCO_BINARY_STATE | ❌ | |
| LAN_X_CV_POM_WRITE_BYTE | ❌ | |
| LAN_X_CV_POM_WRITE_BIT | ❌ | |
| LAN_X_CV_POM_READ_BYTE | ❌ | |
| LAN_X_CV_POM_ACCESSORY_WRITE_BYTE | ❌ | |
| LAN_X_CV_POM_ACCESSORY_WRITE_BIT | ❌ | |
| LAN_X_CV_POM_ACCESSORY_READ_BYTE | ❌ | |
| LAN_X_GET_FIRMWARE_VERSION | ✅ | |
| LAN_SET_BROADCASTFLAGS | ✅ | |
| LAN_GET_BROADCASTFLAGS | ✅ | |
| LAN_GET_LOCOMODE | ✅ | |
| LAN_SET_LOCOMODE | ✅ | |
| LAN_GET_TURNOUTMODE | ✅ | |
| LAN_SET_TURNOUTMODE | ✅ | |
| LAN_RMBUS_GETDATA | ❌ | |
| LAN_RMBUS_PROGRAMMODULE | ❌ | |
| LAN_SYSTEMSTATE_GETDATA | ✅ | |
| LAN_RAILCOM_GETDATA | ❌ | |
| LAN_LOCONET_FROM_LAN | ❌ | |
| LAN_LOCONET_DISPATCH_ADDR | ❌ | |
| LAN_LOCONET_DETECTOR | ❌ | |
| LAN_CAN_DETECTOR | ❌ | |
| LAN_CAN_DEVICE_GET_DESCRIPTION | ❌ | |
| LAN_CAN_DEVICE_SET_DESCRIPTION | ❌ | |
| LAN_CAN_BOOSTER_SET_TRACKPOWER | ❌ | |
| LAN_FAST_CLOCK_CONTROL | ❌ | |
| LAN_FAST_CLOCK_SETTINGS_GET | ❌ | |
| LAN_FAST_CLOCK_SETTINGS_SET | ❌ | |
| LAN_BOOSTER_SET_POWER |❌ | |
| LAN_BOOSTER_GET_DESCRIPTION | ❌ | |
| LAN_BOOSTER_SET_DESCRIPTION | ❌ | |
| LAN_BOOSTER_SYSTEMSTATE_GETDATA | ❌ | |
| LAN_DECODER_GET_DESCRIPTION | ❌ | |
| LAN_DECODER_SET_DESCRIPTION | ❌ | |
| LAN_DECODER_SYSTEMSTATE_GETDATA | ❌ | |
| LAN_ZLINK_GET_HWINFO| ❌ | |

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
 | LAN_X_CV_NACK_SC  | ❌ | |
 | LAN_X_CV_NACK | ❌ | |
 | LAN_X_UNKNOWN_COMMAND  | ✅ | |
 | LAN_X_STATUS_CHANGED  | ✅ | |
 | LAN_X_GET_VERSION  | ✅ | |
 | LAN_X_CV_RESULT  | ❌ | |
 | LAN_X_BC_STOPPED  | ✅ | |
 | LAN_X_LOCO_INFO  | ✅ | |
 | LAN_X_GET_FIRMWARE_VERSION | ✅ | |
 | LAN_GET_BROADCASTFLAGS  | ✅ | |
 | LAN_GET_LOCOMODE | ✅ | |
 | LAN_GET_TURNOUTMODE  | ✅ | |
 | LAN_RMBUS_DATACHANGED  | ❌ | |
 | LAN_SYSTEMSTATE_DATACHANGED | ✅ | |
 | LAN_RAILCOM_DATACHANGED  | ❌ | |
 | LAN_LOCONET_Z21_RX  | ❌ | |
 | LAN_LOCONET_Z21_TX  | ❌ | |
 | LAN_LOCONET_FROM_LAN  | ❌ | |
 | LAN_LOCONET_DISPATCH_ADDR | ❌ | |
 | LAN_LOCONET_DETECTOR  | ❌ | |
 | LAN_CAN_DETECTOR  | ❌ | |
 | LAN_CAN_DEVICE_GET_DESCRIPTION | ❌ | |
 | LAN_CAN_BOOSTER_SYSTEMSTATE_CHGD | ❌ | |
 | LAN_FAST_CLOCK_DATA  | ❌ | |
 | LAN_FAST_CLOCK_SETTINGS_GET  | ❌ | |
 | LAN_BOOSTER_GET_DESCRIPTION  | ❌ | |
 | LAN_BOOSTER_SYSTEMSTATE_DATACHANGED  | ❌ | |
 | LAN_DECODER_GET_DESCRIPTION  | ❌ | |
 | LAN_DECODER_SYSTEMSTATE_DATACHANGED  | ❌ | |
 | LAN_ZLINK_GET_HWINFO  | ❌ | |
 
## Contributing

Contributions are welcome! If you encounter any issues or have suggestions, please open an issue or submit a pull request.

## License

This project is licensed under the GPL-3.0 license - see the LICENSE file for details.
