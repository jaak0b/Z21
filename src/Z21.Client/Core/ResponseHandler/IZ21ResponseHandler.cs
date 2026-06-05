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

    /// <summary>
    /// Length-safe frame matcher shared by every response handler. Returns <c>true</c> only when
    /// <paramref name="response"/> is non-null, at least <paramref name="minimumLength"/> bytes long,
    /// and every <c>(index, value)</c> pair in <paramref name="expected"/> matches. It never throws
    /// on a short or null datagram, replacing the per-handler <c>try/catch (IndexOutOfRangeException)</c>
    /// guard and the duplicated <c>response.Length &gt;= n</c> checks.
    /// </summary>
    public bool MatchesFrame(byte[] response, int minimumLength, params (int Index, byte Value)[] expected)
    {
      if (response is null || response.Length < minimumLength)
        return false;

      foreach ((int index, byte value) in expected)
        if ((uint)index >= (uint)response.Length || response[index] != value)
          return false;

      return true;
    }
  }
}