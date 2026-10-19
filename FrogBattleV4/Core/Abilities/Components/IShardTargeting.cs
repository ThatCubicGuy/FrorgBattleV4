using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.Abilities.Components;

public interface IShardTargeting
{
    [System.Obsolete("RNG will be removed from shard resolution pipeline.")]
    IEnumerable<ShardTargetingContext> SelectTargets(ShardResolutionScope scope, BattleEnvironment env);
    TargetingInstruction GenerateInstruction(ShardResolutionScope scope, BattleEnvironment env);
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