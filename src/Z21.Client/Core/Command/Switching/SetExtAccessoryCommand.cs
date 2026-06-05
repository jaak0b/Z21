using Z21.Core.Codecs;
using Z21.Core.Framing;
using Z21.Core.Model.ExcAccessoryPayload;

namespace Z21.Core.Command.Switching
{
  /// <summary>
  /// From Z21 FW V1. 40, a DCC command in the "extended accessory decoder package format "(DCCext) can be sent to an extended accessory decoder with the following request. It allows to send even switching times for turnouts or complex signal aspects with just one single command
  /// </summary>
  public class SetExtAccessoryCommand : IZ21Command
  {
    /// <summary>
    /// From Z21 FW V1. 40, a DCC command in the "extended accessory decoder package format "(DCCext) can be sent to an extended accessory decoder with the following request. It allows to send even switching times for turnouts or complex signal aspects with just one single command
    /// </summary>
    /// <remarks>The 10837 Z21 signaldecoder interprets <param name="payload"></param> as one of 256 theoretically possible signal aspects. The actual value range depends to a large extent on the signal type set in the signal decoder. See <see href="https://www.z21.eu/en/products/z21-signal-decoder/signaltypen"/> for all possible values.</remarks>
    /// <remarks>The 10836 Z21 switch DECODER interprets the payload as "switch decoder with reception of switching time". Use <see cref="SwitchDecoderPayload"/> to generate payload. </remarks>
    public SetExtAccessoryCommand(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, ushort accessoryAddress, IExcAccessoryPayload payload) : this(frameBuilder, addressCodec, accessoryAddress, (byte)payload.Payload)
    {
    }

    /// <summary>
    /// From Z21 FW V1. 40, a DCC command in the "extended accessory decoder package format" can be sent to an extended accessory decoder with the following request. It allows to send even switching times for turnouts or complex signal aspects with just one single command
    /// </summary>
    /// <remarks>The 10837 Z21 signaldecoder interprets <param name="payload"></param> as one of 256 theoretically possible signal aspects. The actual value range depends to a large extent on the signal type set in the signal decoder. See <see href="https://www.z21.eu/en/products/z21-signal-decoder/signaltypen"/> for all possible values.</remarks>
    /// <remarks>The 10836 Z21 switch DECODER interprets the payload as "switch decoder with reception of switching time". Use <see cref="SwitchDecoderPayload"/> to generate payload. </remarks>
    public SetExtAccessoryCommand(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, ushort accessoryAddress, byte payload)
    {
      (byte lsb, byte msb) = addressCodec.SplitExtAccessoryAddress(accessoryAddress);
      Data = frameBuilder.BuildXBus(0x54, msb, lsb, payload, 0x00);
    }

    public string Name => "LAN_X_SET_EXT_ACCESSORY";

    public byte[] Data { get; }
  }
}
