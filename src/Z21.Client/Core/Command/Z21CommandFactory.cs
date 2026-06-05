using System;
using Microsoft.Extensions.DependencyInjection;
using Z21.Core.Codecs;
using Z21.Core.Framing;

namespace Z21.Core.Command
{
  public class Z21CommandFactory : IZ21CommandFactory
  {
    private readonly IServiceProvider _encodingServices;

    public Z21CommandFactory(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, ILocoSpeedCodec locoSpeedCodec)
    {
      ArgumentNullException.ThrowIfNull(frameBuilder);
      ArgumentNullException.ThrowIfNull(addressCodec);
      ArgumentNullException.ThrowIfNull(locoSpeedCodec);
      _encodingServices = new EncodingServiceProvider(frameBuilder, addressCodec, locoSpeedCodec);
    }

    public TCommand Create<TCommand>(params object[] args) where TCommand : IZ21Command =>
      (TCommand)ActivatorUtilities.CreateInstance(_encodingServices, typeof(TCommand), args);

    private sealed class EncodingServiceProvider(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, ILocoSpeedCodec locoSpeedCodec) : IServiceProvider
    {
      public object? GetService(Type serviceType)
      {
        if (serviceType == typeof(IZ21FrameBuilder))
          return frameBuilder;
        if (serviceType == typeof(IAddressCodec))
          return addressCodec;
        if (serviceType == typeof(ILocoSpeedCodec))
          return locoSpeedCodec;
        return null;
      }
    }
  }
}
