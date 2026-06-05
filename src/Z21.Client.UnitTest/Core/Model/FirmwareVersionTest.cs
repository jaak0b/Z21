using Z21.Core.Model;

namespace Z21.UnitTest.Core.Model
{
  [TestFixture]
  public class FirmwareVersionTests
  {
    [Test]
    public void ToString_ReturnsExpectedFormat()
    {
      FirmwareVersion version = new(3, 6);
      Assert.That(version.ToString(), Is.EqualTo("3.6"));
    }

    [Test]
    public void Equals_SameValues_ReturnsTrue()
    {
      FirmwareVersion v1 = new(4, 0);
      FirmwareVersion v2 = new(4, 0);
      Assert.That(v1, Is.EqualTo(v2));
      Assert.That(v1, Is.EqualTo(v2));
      Assert.That(v1, Is.EqualTo(v2));
    }

    [Test]
    public void Equals_DifferentValues_ReturnsFalse()
    {
      FirmwareVersion v1 = new(3, 0);
      FirmwareVersion v2 = new(3, 1);
      Assert.That(v1, Is.Not.EqualTo(v2));
      Assert.That(v1, Is.Not.EqualTo(v2));
      Assert.That(v1, Is.Not.EqualTo(v2));
    }

    [Test]
    public void CompareTo_MajorVersionWins()
    {
      FirmwareVersion lower = new(3, 9);
      FirmwareVersion higher = new(4, 0);
      Assert.That(lower.CompareTo(higher), Is.LessThan(0));
      Assert.That(higher.CompareTo(lower), Is.GreaterThan(0));
    }

    [Test]
    public void CompareTo_MinorVersionWins()
    {
      FirmwareVersion lower = new(3, 5);
      FirmwareVersion higher = new(3, 6);
      Assert.That(lower.CompareTo(higher), Is.LessThan(0));
      Assert.That(higher.CompareTo(lower), Is.GreaterThan(0));
    }

    [Test]
    public void ComparisonOperators_WorkAsExpected()
    {
      FirmwareVersion v1 = new(3, 0);
      FirmwareVersion v2 = new(3, 6);
      FirmwareVersion v3 = new(4, 0);
      FirmwareVersion v4 = new(4, 1);

      Assert.That(v1, Is.LessThan(v2), "v1 < v2");
      Assert.That(v2, Is.LessThanOrEqualTo(v3), "v2 <= v3");
      Assert.That(v3, Is.GreaterThan(v2), "v3 > v2");
      Assert.That(v4, Is.GreaterThanOrEqualTo(v3), "v4 >= v3");
    }

    [Test]
    public void GetHashCode_EqualObjects_HaveSameHash()
    {
      FirmwareVersion v1 = new(3, 6);
      FirmwareVersion v2 = new(3, 6);
      Assert.That(v2.GetHashCode(), Is.EqualTo(v1.GetHashCode()));
    }

    [Test]
    public void Equals_Null_ReturnsFalse()
    {
      FirmwareVersion version = new(3, 6);
      Assert.That(version, Is.Not.EqualTo(null));
    }

    [Test]
    public void EqualityOperators_WithNull_DoNotThrowAndCompareByReference()
    {
      FirmwareVersion version = new(3, 6);
      FirmwareVersion? @null = null;

      Assert.Multiple(() =>
                      {
                        Assert.That(@null == null, Is.True, "null == null");
                        Assert.That(version == @null, Is.False, "version == null");
                        Assert.That(@null == version, Is.False, "null == version");
                        Assert.That(version != @null, Is.True, "version != null");
                      });
    }

    [Test]
    public void RelationalOperators_WithNull_TreatNullAsLowest()
    {
      FirmwareVersion version = new(1, 0);
      FirmwareVersion? @null = null;
      FirmwareVersion? otherNull = null;

      Assert.Multiple(() =>
                      {
                        Assert.That(@null < version, Is.True, "null < version");
                        Assert.That(version > @null, Is.True, "version > null");
                        Assert.That(version < @null, Is.False, "version < null");
                        Assert.That(@null <= otherNull, Is.True, "null <= null");
                        Assert.That(version >= @null, Is.True, "version >= null");
                      });
    }
  }
}