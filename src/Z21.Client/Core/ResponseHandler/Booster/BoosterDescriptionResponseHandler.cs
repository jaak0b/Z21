using System;
using System.Text;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.Booster
{
  public interface IBoosterDescriptionResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<BoosterDescriptionReceivedEventArgs>? OnBoosterDescriptionReceived;
  }

  /// <summary>
  /// Reports the description of a zLink booster (<c>LAN_BOOSTER_GET_DESCRIPTION</c> reply, protocol §11.2.1).
  /// A leading 0xFF means no description has ever been stored and is reported as an empty string.
  /// </summary>
  public class BoosterDescriptionResponseHandler : IBoosterDescriptionResponseHandler
  {
    private const int NameLength = 32;

    public event EventHandler<BoosterDescriptionReceivedEventArgs>? OnBoosterDescriptionReceived;

    public string Name => "LAN_BOOSTER_GET_DESCRIPTION";

    public bool CanHandle(byte[] response)
    {
      ArgumentNullException.ThrowIfNull(response);
      return response.Length >= 4 + NameLength && response[2] == 0xB8 && response[3] == 0x00;
    }

    public void Handle(byte[] response)
    {
      string name;
      if (response[4] == 0xFF)
        name = string.Empty;
      else
      {
        name = Encoding.Latin1.GetString(response, 4, NameLength);
        int terminator = name.IndexOf('\0');
        if (terminator >= 0)
          name = name[..terminator];
      }
      OnBoosterDescriptionReceived?.Invoke(this, new(name));
    }
  }
}
