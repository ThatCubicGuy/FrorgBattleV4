using System.Linq;
using FrogBattleV4.Core.Abilities.Components.Actions;
using FrogBattleV4.Core.Effects.StatusEffects;

namespace FrogBattleV4.Core.Abilities.Components;

public class EffectCommand : IShardCommand
{
    public required StatusEffectDefinition Definition { get; init; }
    public required int InitialTurns { get; init; }
    public double ApplicationChance { get; init; } = 1;
    public ChanceType ChanceType { get; init; } = ChanceType.Fixed;
    public int AddedStacks { get; init; } = 1;
    public IShardTargeting? Targeting { get; init; }

    public void Generate(ref LinkResolutionState context, LinkResolutionBuilder builder)
    {
        return Targeting
            .SelectTargets(context)
            .Select(atc => new ApplyEffect
            {
                Target = atc.Target,
                Definition = Definition,
                AddedStacks = AddedStacks,
                ApplicationChance = ApplicationChance,
                ChanceType = ChanceType,
                InitialTurns = InitialTurns,
                Rng = context.Rng,
            });
    }
}