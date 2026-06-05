using System.Collections.Generic;
using Z21.Core;
using Z21.Core.Framing;
using Z21.Core.ResponseHandler;

namespace Z21.UnitTest.Core
{
  public class Z21ResponseHandlerTest
  {
    private sealed class RecordingHandler : IZ21ResponseHandler
    {
      private readonly bool _canHandle;
      private readonly bool _throwOnHandle;
      private readonly bool _throwOnCanHandle;

      public RecordingHandler(bool canHandle, bool throwOnHandle = false, bool throwOnCanHandle = false)
      {
        _canHandle = canHandle;
        _throwOnHandle = throwOnHandle;
        _throwOnCanHandle = throwOnCanHandle;
      }

      public List<byte[]> Handled { get; } = [];

      public string Name => "RECORDING";

      public bool CanHandle(byte[] response)
      {
        if (_throwOnCanHandle)
          throw new System.InvalidOperationException("boom in CanHandle");
        return _canHandle;
      }

      public void Handle(byte[] response)
      {
        Handled.Add(response);
        if (_throwOnHandle)
          throw new System.InvalidOperationException("boom");
      }
    }

    [Test]
    public void IncomingBytes_AreFramedAndDispatchedToCapableHandlers()
    {
      FakeTransport transport = new();
      RecordingHandler capable = new(canHandle: true);
      RecordingHandler incapable = new(canHandle: false);
      _ = new Z21ResponseHandler(transport, new Z21FrameReader(), new List<IZ21ResponseHandler> { capable, incapable });

      transport.RaiseBytes([0x07, 0x00, 0x40, 0x00, 0x21, 0x21, 0x00]);

      Assert.Multiple(() =>
                      {
                        Assert.That(capable.Handled, Has.Count.EqualTo(1), "capable handler must receive the frame");
                        Assert.That(capable.Handled[0], Is.EqualTo(new byte[] { 0x07, 0x00, 0x40, 0x00, 0x21, 0x21, 0x00 }));
                        Assert.That(incapable.Handled, Is.Empty, "incapable handler must be skipped");
                      });
    }

    [Test]
    public void HandlerThrowing_DoesNotPropagateAndOtherHandlersStillRun()
    {
      FakeTransport transport = new();
      RecordingHandler throwing = new(canHandle: true, throwOnHandle: true);
      RecordingHandler second = new(canHandle: true);
      _ = new Z21ResponseHandler(transport, new Z21FrameReader(), new List<IZ21ResponseHandler> { throwing, second });

      Assert.DoesNotThrow(() => transport.RaiseBytes([0x07, 0x00, 0x40, 0x00, 0x21, 0x21, 0x00]));
      Assert.That(second.Handled, Has.Count.EqualTo(1), "a throwing handler must not stop the others");
    }

    [Test]
    public void CanHandleThrowing_DoesNotPropagateAndOtherHandlersStillRun()
    {
      FakeTransport transport = new();
      RecordingHandler throwing = new(canHandle: true, throwOnCanHandle: true);
      RecordingHandler second = new(canHandle: true);
      _ = new Z21ResponseHandler(transport, new Z21FrameReader(), new List<IZ21ResponseHandler> { throwing, second });

      Assert.DoesNotThrow(() => transport.RaiseBytes([0x07, 0x00, 0x40, 0x00, 0x21, 0x21, 0x00]));
      Assert.That(second.Handled, Has.Count.EqualTo(1), "a handler whose CanHandle throws must not stop the others");
    }
  }
}
