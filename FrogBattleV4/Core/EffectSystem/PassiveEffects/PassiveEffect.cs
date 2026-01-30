#nullable enable
using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem.PassiveEffects;

public class PassiveEffect : IEffectDefinition, IAttributeModifier
{
    public required string Id { get; init; }
    public required IReadOnlyList<IModifierComponent> Modifiers { get; init; }
    public required IReadOnlyList<IConditionComponent> Conditions { get; init; }

    public uint GetStacks(EffectContext ctx)
    {
        throw new System.NotImplementedException();
    }
    public double GetModifiedStat(double currentValue, EffectContext ctx)
    {
        throw new System.NotImplementedException();
    }
}