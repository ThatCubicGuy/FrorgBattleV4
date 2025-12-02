#nullable enable
using System.Linq;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem.ActiveEffects;

public class ActiveEffectInstance : IAttributeModifier
{
    public required ActiveEffectDefinition Definition { get; init; }
    public ICharacter? Source { get; init; }
    public uint Turns { get; set; }
    public uint Stacks { get; set; }

    public double GetModifiedStat(string statName, double currentValue, EffectContext ctx)
    {
        return Definition.Modifiers.Where(x => x.Modifies(statName))
            .Aggregate(currentValue, (x, y) => y.Apply(x, ctx));
    }
}