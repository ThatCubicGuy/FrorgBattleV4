#nullable enable
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrogBattleV4.Core.Effects;
using FrogBattleV4.Core.Effects.StatusEffects;

namespace FrogBattleV4.Core.AbilitySystem.Components;

public class EffectComponent : IAbilityCommandComponent
{
    public required StatusEffectDefinition Definition { get; init; }
    public required int InitialTurns { get; init; }
    public double ApplicationChance { get; init; } = 1;
    public ChanceType ChanceType { get; init; } = ChanceType.Fixed;
    public int AddedStacks { get; init; } = 1;
    public ITargetingComponent? Targeting { get; init; }

    public IEnumerable<IBattleCommand> GetContribution(AbilityExecContext ctx)
    {
        return ((Targeting ?? ctx.Definition.Targeting)
            .SelectTargets(ctx)
            .Select(atc => new ApplyEffectCommand
            {
                Target = atc.Target,
                EffectSource = ctx.User,
                Definition = Definition,
                AddedStacks = AddedStacks,
                ApplicationChance = ApplicationChance,
                ChanceType = ChanceType,
                InitialTurns = InitialTurns,
                Rng = ctx.Rng,
            }));
    }
}