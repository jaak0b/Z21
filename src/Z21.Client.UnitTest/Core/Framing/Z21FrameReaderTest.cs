using System.Collections.Generic;
using CommandStation.Framing;
using Z21.Core.Framing;

namespace Z21.UnitTest.Core.Framing
{
  [TestFixture]
  public class Z21FrameReaderTest
  {
    private Z21FrameReader _reader = null!;
    private List<byte[]> _frames = null!;

    [SetUp]
    public void SetUp()
    {
      _reader = new Z21FrameReader();
      _frames = [];
      _reader.OnFrameReceived += (_, args) => _frames.Add(args.Frame);
    }

    [Test]
    public void Append_SingleCompleteFrame_EmitsThatFrame()
    {
      byte[] frame = [0x07, 0x00, 0x40, 0x00, 0x21, 0x21, 0x00];

      _reader.Append(frame);

      Assert.That(_frames, Has.Count.EqualTo(1));
      Assert.That(_frames[0], Is.EqualTo(frame));
    }

    [Test]
    public void Append_TwoFramesInOneChunk_EmitsBothInOrder()
    {
      byte[] first = [0x07, 0x00, 0x40, 0x00, 0x21, 0x21, 0x00];
      byte[] second = [0x04, 0x00, 0x12, 0x34];
      byte[] combined = [.. first, .. second];

      _reader.Append(combined);

      Assert.That(_frames, Has.Count.EqualTo(2));
      Assert.That(_frames[0], Is.EqualTo(first));
      Assert.That(_frames[1], Is.EqualTo(second));
    }

    [Test]
    public void Append_FrameSplitAcrossChunks_EmitsOnceComplete()
    {
      _reader.Append(new byte[] { 0x07, 0x00, 0x40, 0x00 });
      Assert.That(_frames, Is.Empty);

      _reader.Append(new byte[] { 0x21, 0x21, 0x00 });

      Assert.That(_frames, Has.Count.EqualTo(1));
      Assert.That(_frames[0], Is.EqualTo(new byte[] { 0x07, 0x00, 0x40, 0x00, 0x21, 0x21, 0x00 }));
    }

    [Test]
    public void Append_LengthPrefixSplitAcrossChunks_StillReassembles()
    {
      _reader.Append(new byte[] { 0x07 });
      Assert.That(_frames, Is.Empty);

      _reader.Append(new byte[] { 0x00, 0x40, 0x00, 0x21, 0x21, 0x00 });

      Assert.That(_frames, Has.Count.EqualTo(1));
      Assert.That(_frames[0], Is.EqualTo(new byte[] { 0x07, 0x00, 0x40, 0x00, 0x21, 0x21, 0x00 }));
    }

    [Test]
    public void Append_Null_ThrowsArgumentNullException()
    {
      Assert.Throws<System.ArgumentNullException>(() => _reader.Append(null!));
    }

    [Test]
    public void Append_ZeroLengthFrame_DiscardsBufferAndEmitsNothing()
    {
      _reader.Append(new byte[] { 0x00, 0x00, 0xAA, 0xBB });

      Assert.That(_frames, Is.Empty);

      // A subsequent valid frame is processed (buffer was cleared, not stuck).
      _reader.Append(new byte[] { 0x04, 0x00, 0x12, 0x34 });
      Assert.That(_frames, Has.Count.EqualTo(1));
      Assert.That(_frames[0], Is.EqualTo(new byte[] { 0x04, 0x00, 0x12, 0x34 }));
    }

    [Test]
    public void Append_LengthPrefixExceedsMaxFrame_DiscardsAndResyncs()
    {
      // DataLen = 0x2000 (8192) is far beyond the 1472-byte IPv4 payload limit: a corrupt prefix
      // must not make the reader buffer indefinitely waiting for bytes that will never arrive.
      _reader.Append(new byte[] { 0x00, 0x20, 0xAA, 0xBB });
      Assert.That(_frames, Is.Empty);

      // A subsequent valid frame is still processed (buffer was cleared, not stuck).
      _reader.Append(new byte[] { 0x04, 0x00, 0x12, 0x34 });
      Assert.That(_frames, Has.Count.EqualTo(1));
      Assert.That(_frames[0], Is.EqualTo(new byte[] { 0x04, 0x00, 0x12, 0x34 }));
    }

    [Test]
    public void Append_FrameWithHighByteLength_UsesBothLengthBytes()
    {
      byte[] frame = new byte[256];
      frame[0] = 0x00; // low byte of length
      frame[1] = 0x01; // high byte of length => 0x0100 = 256
      frame[2] = 0x40;

      _reader.Append(frame);

      Assert.That(_frames, Has.Count.EqualTo(1));
      Assert.That(_frames[0], Has.Length.EqualTo(256));
    }

    [Test]
    public void Append_TrailingPartialFrame_RetainedUntilCompleted()
    {
      byte[] complete = [0x04, 0x00, 0x12, 0x34];
      byte[] partial = [0x05, 0x00, 0xAA];
      _reader.Append([.. complete, .. partial]);

      Assert.That(_frames, Has.Count.EqualTo(1));
      Assert.That(_frames[0], Is.EqualTo(complete));

      _reader.Append(new byte[] { 0xBB, 0xCC });

      Assert.That(_frames, Has.Count.EqualTo(2));
      Assert.That(_frames[1], Is.EqualTo(new byte[] { 0x05, 0x00, 0xAA, 0xBB, 0xCC }));
    }
  }
}
