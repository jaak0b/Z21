using System;

namespace Z21.Core.Exception
{
  /// <summary>
  /// Thrown when a command is sent before the command station has been connected.
  /// </summary>
  public class NotConnectedException(string message) : InvalidOperationException(message)
  {
  }
}
