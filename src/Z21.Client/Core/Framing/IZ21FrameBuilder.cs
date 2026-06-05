namespace Z21.Core.Framing
{
  /// <summary>
  /// Assembles outbound Z21 LAN frames, prepending the little-endian DataLen prefix and (for X-Bus
  /// frames) appending the trailing XOR checksum.
  /// </summary>
  public interface IZ21FrameBuilder
  {
    /// <summary>
    /// Builds a plain LAN frame: <c>[DataLen][header][payload]</c>, with no checksum.
    /// </summary>
    byte[] BuildLan(ushort header, params byte[] payload);

    /// <summary>
    /// Builds an X-Bus frame under LAN header <c>0x40 0x00</c>: <c>[DataLen][0x40 0x00][xHeader][data][XOR]</c>,
    /// where the XOR runs over the X-header and data bytes.
    /// </summary>
    byte[] BuildXBus(byte xHeader, params byte[] data);

    /// <summary>
    /// Builds a LAN frame that carries a trailing XOR checksum over its data bytes (used by non X-Bus
    /// LAN messages such as <c>LAN_FAST_CLOCK_CONTROL</c>): <c>[DataLen][header][data][XOR]</c>.
    /// </summary>
    byte[] BuildLanChecksummed(ushort header, params byte[] data);
  }
}
