using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Z21.Core.Command;
using Z21.Core.Command.SystemState;
using Z21.Core.Exception;
using Z21.Core.Helper;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;
using Z21.Transport;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Z21.Core
{

  public class Z21Client : IZ21Client
  {
    private readonly ILogger<Z21Client>? _logger;
    private readonly Z21Configuration _z21Configuration;
    private readonly IZ21Transport _transport;
    private readonly DelayedAction _delayedKeepAliveAction;
    private readonly Z21Watchdog _z21Watchdog;
    
    /// <summary>
    /// IPv4 safe MTU for payload according to specification.
    /// </summary>
    public const int MaxUdpPayload = 1472;

    /// <exception cref="PlatformNotSupportedException">Thrown when system architecture is not little-endian.</exception>
    public Z21Client(Z21Configuration z21Configuration, IZ21Transport z21Transport, ILogger<Z21Client>? logger = null)
    {
      ArgumentNullException.ThrowIfNull(z21Configuration);
      ArgumentNullException.ThrowIfNull(z21Transport);

      if (!BitConverter.IsLittleEndian)
        throw new PlatformNotSupportedException("Z21Client requires little-endian architecture.");

      _z21Configuration = z21Configuration;
      _transport = z21Transport;
      _logger = logger;
      _z21Watchdog = new (z21Configuration);
      _z21Watchdog.OnReachabilityChanged += async (_, args ) => await Watchdog_OnOnReachabilityChanged(args);
      _delayedKeepAliveAction = new (TimeSpan.FromSeconds(45), async () => await SendCommandsAsync(new GetFirmwareVersionCommand()));
    }

    public event EventHandler<ConnectionChangedEventArgs>? OnConnectionChanged;

    public bool IsConnected { get; private set; }
    
    public async Task ConnectAsync()
    {
      _logger?.LogInformation("Z21Client trying to connect with {ClientIPEndPoint}.", _transport.Z21Configuration.ClientIPEndPoint);
      _transport.Connect();
      await LogOnAsync();
    }

    public async Task SendCommandsAsync(params IZ21Command[] z21Commands)
    {
      ArgumentNullException.ThrowIfNull(z21Commands);

      if (!_transport.IsConnected)
        await ConnectAsync();

      foreach (var z21Command in z21Commands)
        _logger?.LogDebug("{commandName} sending {datagram} to Z21.", z21Command.Name, BitConverter.ToString(z21Command.Data));

      var combinedPayload = z21Commands.SelectMany(z21Command => z21Command.Data).ToArray();
      MtuPayloadLengthExceededException.ThrowIfExceeded(combinedPayload);

      await _transport.SendAsync(combinedPayload);
      _delayedKeepAliveAction.Delay();
    }

    protected async virtual Task LogOnAsync()
    {
      await SendCommandsAsync(new SetBroadcastFlagsCommand(_z21Configuration.BroadcastFlags), new GetFirmwareVersionCommand());
    }

    private async Task Watchdog_OnOnReachabilityChanged(ConnectionChangedEventArgs args)
    {
      if (args.IsConnected)
        await LogOnAsync();
      IsConnected = args.IsConnected;
      OnConnectionChanged?.Invoke(this, args);
    }
  }
}