namespace Z21.Core.Command
{
  public interface IZ21Command
  {
    /// <summary>
    /// Human-readable command according to the specification.
    /// </summary>
    public string Name { get; }
    
    public byte[] Data { get; }
  }
}