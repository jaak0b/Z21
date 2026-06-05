using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Z21.Core.Codecs;
using Z21.Core.Command.Driving;
using Z21.Core.Command.SystemState;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.Driving
{
  public interface ILocoInfoResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<LocoInfoReceivedEventArgs>? OnLocoInfoReceived;
  }

  /// <summary>
  /// This message is sent from the Z21 to the clients in response to the command <see cref="GetLocoInfoCommand"/>.
  /// <para/>It is also unsolicitedly sent to an associated client if the locomotive status has been changed by one of the (other) clients or handset controls and the associated client has activated the corresponding broadcast <see cref="Z21BroadcastFlags.DriveAndSwitchingMessages"/> or <see cref="Z21BroadcastFlags.LocoInfoChangedMessages"/> via <see cref="SetBroadcastFlagsCommand"/>.
  /// </summary>
  public class LocoInfoResponseHandler : ILocoInfoResponseHandler
  {
    private readonly ILocoSpeedCodec _locoSpeedCodec;
    private readonly ILogger<LocoInfoResponseHandler>? _logger;

    public LocoInfoResponseHandler(ILocoSpeedCodec locoSpeedCodec, ILogger<LocoInfoResponseHandler>? logger = null)
    {
      _locoSpeedCodec = locoSpeedCodec;
      _logger = logger;
    }

    public string Name => "LAN_X_LOCO_INFO";

    public event EventHandler<LocoInfoReceivedEventArgs>? OnLocoInfoReceived;

    public bool CanHandle(byte[] response) =>
      ((IZ21ResponseHandler)this).MatchesFrame(response, 5, (2, 0x40), (3, 0x00), (4, 0xEF));

    public void Handle(byte[] response)
    {
      ushort address = (ushort)(((response[5] & 0x3F) << 8) + response[6]);

      byte db2 = response[7];
      DecoderMode decoderMode = (db2 & 0x10) == 0x10 ? DecoderMode.MM : DecoderMode.DCC;
      bool locoIsBusy = (db2 & 0x8) == 0x8;

      DccSpeedMode speedMode = (db2 & 0x07) switch
                               {
                                 0x02 => DccSpeedMode.Steps28,
                                 0x04 => DccSpeedMode.Steps128,
                                 _ => DccSpeedMode.Steps14
                               };

      byte db3 = response[8];
      DrivingDirection drivingDirection = (db3 & 0x80) == 0x80 ? DrivingDirection.Forward : DrivingDirection.Backward;
      ushort stepSpeed = _locoSpeedCodec.CalculateSpeedStep(speedMode, (ushort)(db3 & 0x7F));

      byte db4 = response[9];
      bool locoContainedInDoubleTraction = (db4 & 0x40) == 0x40;
      bool smartSearch = (db4 & 0x20) == 0x20;

      List<LocoFunctionData> infodata =
      [
        new(0, GetFunctionToggleType((db4 & 0x10) == 0x10)),
        new(4, GetFunctionToggleType((db4 & 0x8) == 0x8)),
        new(3, GetFunctionToggleType((db4 & 0x4) == 0x4)),
        new(2, GetFunctionToggleType((db4 & 0x2) == 0x2)),
        new(1, GetFunctionToggleType((db4 & 0x1) == 0x1))
      ];

      int functionAddressCount = 5;
      for (int index = 10; index < response.Length - 1; index++)
      {
        BitArray functionBits = new(new[] { response[index] });
        for (int temp = 0; temp < 8; temp++)
        {
          infodata.Add(new((short)functionAddressCount++, GetFunctionToggleType(functionBits.Get(temp))));
        }
      }

      _logger?.LogDebug("{name} address {address}, decoderMode {decoderMode}, busy {busy}, speedMode {speedMode}, direction {direction}, speed {speed}, doubleTraction {doubleTraction}, smartSearch {smartSearch}.",
                        Name, address, decoderMode, locoIsBusy, speedMode, drivingDirection, stepSpeed, locoContainedInDoubleTraction, smartSearch);

      OnLocoInfoReceived?.Invoke(this,
                                 new(new()
                                     {
                                       LocoAddress = address,
                                       LocoFunctionsData = infodata.AsReadOnly(),
                                       DccSpeedMode = speedMode,
                                       DecoderMode = decoderMode,
                                       DrivingDirection = drivingDirection,
                                       LocoSpeed = stepSpeed,
                                       LocoIsBusy = locoIsBusy,
                                       LocoContainedInDoubleTraction = locoContainedInDoubleTraction,
                                       SmartSearch = smartSearch
                                     }));
    }

    private FunctionToggleType GetFunctionToggleType(bool value) => value ? FunctionToggleType.On : FunctionToggleType.Off;
  }
}