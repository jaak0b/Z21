using System;

namespace Z21.Core.Model
{
  public class LocoFunctionData(short functionIndex, FunctionToggleType functionToggleType) : IEquatable<LocoFunctionData>
  {
    public short FunctionIndex { get; } = functionIndex;

    public FunctionToggleType FunctionToggleType { get; } = functionToggleType;

    public bool Equals(LocoFunctionData? other) => other != null && other.FunctionIndex == FunctionIndex;

    override public bool Equals(object? obj) => obj is LocoFunctionData functionData && Equals(functionData);

    override public int GetHashCode() => HashCode.Combine(FunctionIndex);
  }
}