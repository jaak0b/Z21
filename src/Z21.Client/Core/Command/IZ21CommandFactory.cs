namespace Z21.Core.Command
{
  /// <summary>
  /// Constructs <see cref="IZ21Command"/> instances, supplying their required encoding services and
  /// binding any remaining constructor arguments. Adding a new command requires no change here.
  /// </summary>
  public interface IZ21CommandFactory
  {
    /// <summary>
    /// Creates a command of type <typeparamref name="TCommand"/>; encoding services are supplied automatically and <paramref name="args"/> fills the remaining constructor parameters.
    /// </summary>
    TCommand Create<TCommand>(params object[] args) where TCommand : IZ21Command;
  }
}
