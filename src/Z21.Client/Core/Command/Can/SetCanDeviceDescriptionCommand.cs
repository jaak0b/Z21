using System;
using System.Text;
using Z21.Core.Framing;

namespace Z21.Core.Command.Can
{
  /// <summary>
  /// From Z21 FW version 1.41, overwrites the free-text description of a CAN booster
  /// (<c>LAN_CAN_DEVICE_SET_DESCRIPTION</c>, protocol §10.2.2). The name is ISO 8859-1, truncated to and
  /// padded to 16 bytes; the characters <c>"</c> and <c>\</c> are not allowed.
  /// </summary>
  public class SetCanDeviceDescriptionCommand : IZ21Command
  {
    private const int NameLength = 16;

    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> contains a forbidden character.</exception>
    public SetCanDeviceDescriptionCommand(IZ21FrameBuilder frameBuilder, ushort networkId, string name)
    {
      ArgumentNullException.ThrowIfNull(name);
      if (name.Contains('"') || name.Contains('\\'))
        throw new ArgumentException("The characters '\"' and '\\' are not allowed in a device description.", nameof(name));

      byte[] nameBuffer = new byte[NameLength];
      byte[] encoded = Encoding.Latin1.GetBytes(name);
      Array.Copy(encoded, 0, nameBuffer, 0, Math.Min(encoded.Length, NameLength));

      byte[] nid = BitConverter.GetBytes(networkId);
      byte[] payload = new byte[2 + NameLength];
      payload[0] = nid[0];
      payload[1] = nid[1];
      Array.Copy(nameBuffer, 0, payload, 2, NameLength);

      Data = frameBuilder.BuildLan(0x00C9, payload);
    }

    public string Name => "LAN_CAN_DEVICE_SET_DESCRIPTION";

    public byte[] Data { get; }
  }
}
