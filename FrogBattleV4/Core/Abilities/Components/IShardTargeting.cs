using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Abilities.Components;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(Targeting.AreaOfEffectTargeting), "area_of_effect")]
[JsonDerivedType(typeof(Targeting.BlastTargeting), "blast")]
[JsonDerivedType(typeof(Targeting.BounceTargeting), "bounce")]
[JsonDerivedType(typeof(Targeting.FilterTargeting), "filter")]
public interface IShardTargeting
{
    TargetingInstruction GenerateInstruction(ShardTargetingContext targeting, BattleEnvironment env);
}

public abstract record TargetingInstruction
{
    public static readonly TargetingInstruction Empty = new Fixed([]);

    private TargetingInstruction()
    {
    }

    /// <summary>
    /// Returns a fixed targeting instruction with the specified targets.
    /// </summary>
    /// <param name="fixedTargets">Targets to create the instruction from.</param>
    /// <returns>A new instance of <see cref="TargetingInstruction.Fixed"/>.</returns>
    public static TargetingInstruction From(params ShardTargetingContext[] fixedTargets)
    {
        return new Fixed(fixedTargets);
    }

    public TargetingInstruction WithFixed(IEnumerable<ShardTargetingContext> targets)
    {
        return new Mixed([this, new Fixed(targets)]);
    }

    public TargetingInstruction WithRandom(IEnumerable<ShardTargetingContext> targets, int count)
    {
        return new Mixed([this, new Random(targets, count)]);
    }

    public abstract IEnumerable<ShardTargetingContext> Resolve(IBattleRng rng);

    public sealed record Fixed(IEnumerable<ShardTargetingContext> Targets) : TargetingInstruction
    {
        public override IEnumerable<ShardTargetingContext> Resolve(IBattleRng rng)
        {
            return Targets;
        }
    }

    public sealed record Random(IEnumerable<ShardTargetingContext> Candidates, int Count) : TargetingInstruction
    {
        public override IEnumerable<ShardTargetingContext> Resolve(IBattleRng rng)
        {
            for (var i = 0; i < Count; i++)
            {
                yield return rng.Choose(Candidates);
            }
        }
    }

    public sealed record Shuffled(IEnumerable<ShardTargetingContext> Targets, int Count) : TargetingInstruction
    {
        public override IEnumerable<ShardTargetingContext> Resolve(IBattleRng rng)
        {
            return rng.Shuffle(Targets).Take(Count);
        }
    }

    public sealed record Mixed(IEnumerable<TargetingInstruction> Instructions) : TargetingInstruction
    {
        public override IEnumerable<ShardTargetingContext> Resolve(IBattleRng rng)
            => Instructions.SelectMany(i => i.Resolve(rng));
    }
}