using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.Abilities;

public abstract record TargetingInstruction
{
    public static readonly TargetingInstruction Empty = new Fixed([]);

    private TargetingInstruction() { }

    /// <summary>
    /// Returns a fixed targeting instruction with the specified targets.
    /// </summary>
    /// <param name="fixedTargets">Targets to create the instruction from.</param>
    /// <returns>A new instance of <see cref="TargetingInstruction.Fixed"/>.</returns>
    public static TargetingInstruction From(params TargetingContext[] fixedTargets)
    {
        return new Fixed(fixedTargets);
    }

    public TargetingInstruction WithFixed(IEnumerable<TargetingContext> targets)
    {
        return new Mixed([this, new Fixed(targets)]);
    }

    public TargetingInstruction WithRandom(IEnumerable<TargetingContext> targets, int count)
    {
        return new Mixed([this, new Random(targets, count)]);
    }

    public abstract IEnumerable<TargetingContext> Resolve(IBattleRng rng);

    public sealed record Fixed(IEnumerable<TargetingContext> Targets) : TargetingInstruction
    {
        public override IEnumerable<TargetingContext> Resolve(IBattleRng rng)
        {
            return Targets;
        }
    }

    public sealed record Random(IEnumerable<TargetingContext> Candidates, int Count) : TargetingInstruction
    {
        public override IEnumerable<TargetingContext> Resolve(IBattleRng rng)
        {
            for (var i = 0; i < Count; i++)
            {
                yield return rng.Choose(Candidates);
            }
        }
    }

    public sealed record Shuffled(IEnumerable<TargetingContext> Targets, int Count) : TargetingInstruction
    {
        public override IEnumerable<TargetingContext> Resolve(IBattleRng rng)
        {
            return rng.Shuffle(Targets).Take(Count);
        }
    }

    public sealed record Mixed(IEnumerable<TargetingInstruction> Instructions) : TargetingInstruction
    {
        public override IEnumerable<TargetingContext> Resolve(IBattleRng rng)
            => Instructions.SelectMany(i => i.Resolve(rng));
    }
}