using Z21.Core.Model;
using Z21.Core.Model.ExcAccessoryPayload;

namespace Z21.UnitTest.Core.Model.ExcAccessoryPayload
{
  public class SwitchDecoderPayloadTest
  {
    [Test]
    public void Ctor_CalculatesPayloadCorrectly()
    {
      SwitchDecoderPayload payload = new(AccessoryOutput.Output1, 10);
      Assert.That(payload.Payload, Is.EqualTo(0xA));
    }

    [Test]
    public void Ctor_SwitchTimeBiggerThen127_ThrowsArgumentOutOfRangeException()
    {
      Assert.Throws<ArgumentOutOfRangeException>(() => _ = new SwitchDecoderPayload(AccessoryOutput.Output1, 128));
    }
  }
}