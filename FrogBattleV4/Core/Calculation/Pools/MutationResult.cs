using FrogBattleV4.Core.Contexts;

namespace FrogBattleV4.Core.Calculation.Pools;

/// <summary>
/// A fully calculated instance of a specific pool mutation. The raw value of <paramref name="FinalDeltaValue"/>
/// is added to <paramref name="ResultTarget"/>.
/// </summary>
/// <param name="ResultTarget">The pool component that is going to be mutated.</param>
/// <param name="FinalDeltaValue">Final value that will be added to the given pool.</param>
public record MutationResult(PoolId ResultTarget, double FinalDeltaValue, bool Satisfiable) : IResultContext<PoolId>;