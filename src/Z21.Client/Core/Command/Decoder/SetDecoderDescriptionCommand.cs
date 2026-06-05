using System;
using System.Text;
using Z21.Core.Framing;

namespace Z21.Core.Command.Decoder
{
  /// <summary>
  /// Overwrites the description of a zLink decoder (<c>LAN_DECODER_SET_DESCRIPTION</c>, protocol §11.3.2).
  /// The name is ISO 8859-1, truncated/padded to 32 bytes; the characters <c>"</c> and <c>\</c> are not allowed.
  /// </summary>
  public class SetDecoderDescriptionCommand : IZ21Command
  {
    private const int NameLength = 32;

    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> contains a forbidden character.</exception>
    public SetDecoderDescriptionCommand(IZ21FrameBuilder frameBuilder, string name)
    {
      ArgumentNullException.ThrowIfNull(name);
      if (name.Contains('"') || name.Contains('\\'))
        throw new ArgumentException("The characters '\"' and '\\' are not allowed in a decoder description.", nameof(name));

      byte[] nameBuffer = new byte[NameLength];
      byte[] encoded = Encoding.Latin1.GetBytes(name);
      Array.Copy(encoded, 0, nameBuffer, 0, Math.Min(encoded.Length, NameLength));

      Data = frameBuilder.BuildLan(0x00D9, nameBuffer);
    }

    public string Name => "LAN_DECODER_SET_DESCRIPTION";

    public byte[] Data { get; }
  }
}
