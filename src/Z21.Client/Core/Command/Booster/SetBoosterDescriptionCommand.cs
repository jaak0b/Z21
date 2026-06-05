using System;
using System.Text;
using Z21.Core.Framing;

namespace Z21.Core.Command.Booster
{
  /// <summary>
  /// Overwrites the description of a zLink booster (<c>LAN_BOOSTER_SET_DESCRIPTION</c>, protocol §11.2.2).
  /// The name is ISO 8859-1, truncated/padded to 32 bytes; the characters <c>"</c> and <c>\</c> are not allowed.
  /// </summary>
  public class SetBoosterDescriptionCommand : IZ21Command
  {
    private const int NameLength = 32;

    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> contains a forbidden character.</exception>
    public SetBoosterDescriptionCommand(IZ21FrameBuilder frameBuilder, string name)
    {
      ArgumentNullException.ThrowIfNull(name);
      if (name.Contains('"') || name.Contains('\\'))
        throw new ArgumentException("The characters '\"' and '\\' are not allowed in a booster description.", nameof(name));

      byte[] nameBuffer = new byte[NameLength];
      byte[] encoded = Encoding.Latin1.GetBytes(name);
      Array.Copy(encoded, 0, nameBuffer, 0, Math.Min(encoded.Length, NameLength));

      Data = frameBuilder.BuildLan(0x00B9, nameBuffer);
    }

    public string Name => "LAN_BOOSTER_SET_DESCRIPTION";

    public byte[] Data { get; }
  }
}
