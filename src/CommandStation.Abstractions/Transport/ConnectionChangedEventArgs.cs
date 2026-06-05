using System;

namespace CommandStation.Transport
{
  public class ConnectionChangedEventArgs : EventArgs
  {
    public ConnectionChangedEventArgs(bool isConnected)
    {
      IsConnected = isConnected;
    }

    public bool IsConnected { get; }
  }
}
