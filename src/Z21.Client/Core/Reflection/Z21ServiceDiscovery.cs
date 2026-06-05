using System;
using System.Collections.Generic;
using System.Linq;

namespace Z21.Core.Reflection
{
  /// <summary>
  /// Discovers concrete implementations of a Z21 service contract (response handlers, parsers) for
  /// container registration, so both DI containers register an identical set.
  /// </summary>
  public sealed class Z21ServiceDiscovery
  {
    /// <summary>
    /// Returns every concrete, non-abstract class in the Z21 assembly that implements <paramref name="baseInterface"/>.
    /// </summary>
    public IEnumerable<Type> GetImplementations(Type baseInterface) =>
      baseInterface.Assembly
                   .GetTypes()
                   .Where(type => type is { IsClass: true, IsAbstract: false } && baseInterface.IsAssignableFrom(type));

    /// <summary>
    /// Returns the contract interfaces an implementation should be registered against. The base contract
    /// itself is included only when <paramref name="includeBaseInterface"/> is true (handlers are resolved
    /// as <c>IEnumerable&lt;base&gt;</c> by the dispatcher; parsers are not).
    /// </summary>
    public IEnumerable<Type> GetServiceInterfaces(Type implementationType, Type baseInterface, bool includeBaseInterface) =>
      implementationType.GetInterfaces()
                        .Where(serviceInterface => baseInterface.IsAssignableFrom(serviceInterface) && (includeBaseInterface || serviceInterface != baseInterface));
  }
}
