using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Combat;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

/// <summary>
/// Represents an object that modifies the value of a pool at certain points in time.
/// </summary>
public interface IMutatorComponent
{
    // TODO (Nova): Don't pass mutable status effect instance as parameter
    void Tick(TurnMoment moment, StatusEffectInstance effect, BattleEnvironment env, CommandBuilder builder);
}