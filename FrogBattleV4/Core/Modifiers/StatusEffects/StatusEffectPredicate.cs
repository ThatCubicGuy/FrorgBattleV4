using System.Collections.Immutable;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public class StatusEffectPredicate : IGamePredicate<StatusEffectInstance>, IGamePredicate<EffectAppliedEventArgs>
{
    public ImmutableHashSet<StatusEffectType>? AllowedTypes { get; init; }
    public NumberPredicate? Turns { get; init; }
    public NumberPredicate? Stacks { get; init; }
    public virtual bool Evaluate(StatusEffectInstance value)
    {
        return (AllowedTypes is null || AllowedTypes.Contains(value.Definition.Type)) &&
               (Turns.SafeEvaluate(value.Turns)) &&
               (Stacks.SafeEvaluate(value.Stacks));
    }

    public bool Evaluate(EffectAppliedEventArgs value)
    {
        return (AllowedTypes is null || AllowedTypes.Contains(value.EffectType)) &&
               (Turns.SafeEvaluate(value.Turns)) &&
               (Stacks.SafeEvaluate(value.Stacks));
    }
}