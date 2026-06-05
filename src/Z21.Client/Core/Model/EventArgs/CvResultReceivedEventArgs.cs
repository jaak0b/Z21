namespace Z21.Core.Model.EventArgs
{
  /// <summary>
  /// Carries a positive CV programming result (<c>LAN_X_CV_RESULT</c>): the CV address (0 = CV1) and its value.
  /// </summary>
  public class CvResultReceivedEventArgs(ushort cvAddress, byte value) : System.EventArgs
  {
    public ushort CvAddress { get; } = cvAddress;

    public byte Value { get; } = value;
  }
}
