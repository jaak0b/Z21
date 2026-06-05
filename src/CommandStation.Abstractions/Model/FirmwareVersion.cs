using System;

namespace CommandStation.Model
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
      if (other is null)
        return 1;
      int majorCmp = Major.CompareTo(other.Major);
      return majorCmp != 0 ? majorCmp : Minor.CompareTo(other.Minor);
    }

    public static bool operator <(FirmwareVersion? left, FirmwareVersion? right) => left is null ? right is not null : left.CompareTo(right) < 0;

    public static bool operator >(FirmwareVersion? left, FirmwareVersion? right) => left is not null && left.CompareTo(right) > 0;

    public static bool operator <=(FirmwareVersion? left, FirmwareVersion? right) => left is null || left.CompareTo(right) <= 0;

    public static bool operator >=(FirmwareVersion? left, FirmwareVersion? right) => left is null ? right is null : left.CompareTo(right) >= 0;

    public static bool operator ==(FirmwareVersion? left, FirmwareVersion? right) => Equals(left, right);

    public static bool operator !=(FirmwareVersion? left, FirmwareVersion? right) => !Equals(left, right);
  }
}
