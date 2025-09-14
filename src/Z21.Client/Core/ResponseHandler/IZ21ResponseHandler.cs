using System;

namespace Z21.Core.ResponseHandler
{
  public interface IZ21ResponseHandler
  {
    /// <summary>
    /// Human-readable command according to the specification.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Returns true if this response handler can handle the response. False otherwise.
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    public bool CanHandle(byte[] response);

    public void Handle(byte[] response);
  }
}