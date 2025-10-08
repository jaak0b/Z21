using System;

namespace Z21.Core.Model
{
  public sealed class FirmwareVersion(int major, int minor) : IComparable<FirmwareVersion>, IEquatable<FirmwareVersion>
  {
    public int Major { get; } = major;

    public int Minor { get; } = minor;

    public string Firmware { get; } = major + "." + minor;

    override public string ToString() => Firmware;

    public bool Equals(FirmwareVersion? other) => Major == other?.Major && Minor == other.Minor;

    override public bool Equals(object? obj) => obj is FirmwareVersion other && Equals(other);

    override public int GetHashCode() => HashCode.Combine(Major, Minor);

    public int CompareTo(FirmwareVersion? other)
    {
      int majorCmp = Major.CompareTo(other?.Major);
      return majorCmp != 0 ? majorCmp : Minor.CompareTo(other?.Minor);
    }

    public static bool operator <(FirmwareVersion left, FirmwareVersion right) => left.CompareTo(right) < 0;

    public static bool operator >(FirmwareVersion left, FirmwareVersion right) => left.CompareTo(right) > 0;

    public static bool operator <=(FirmwareVersion left, FirmwareVersion right) => left.CompareTo(right) <= 0;

    public static bool operator >=(FirmwareVersion left, FirmwareVersion right) => left.CompareTo(right) >= 0;

    public static bool operator ==(FirmwareVersion left, FirmwareVersion right) => left.Equals(right);

    public static bool operator !=(FirmwareVersion left, FirmwareVersion right) => !left.Equals(right);
  }
}