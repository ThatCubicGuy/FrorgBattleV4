using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json.Serialization;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Commands;

namespace FrogBattleV4.Core.Abilities.Reactions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "function")]
[JsonDerivedType(typeof(HealReactionFunction), "heal")]
[JsonDerivedType(typeof(ExecuteLinkReactionFunction), "execute_link")]
[JsonDerivedType(typeof(SequenceReactionFunction), "sequence")]
[JsonDerivedType(typeof(ModifyCounterReactionFunction), "modify_counter")]
public abstract class ReactionFunction
{
    public abstract void HandleEvent(EntityUid holder, BattleEnvironment env, EventArgs args);
}

public abstract class CommandReactionFunction : ReactionFunction
{
    [JsonPropertyName("entity")]
    public FunctionTarget AffectedEntity { get; init; }

    public override void HandleEvent(EntityUid holder, BattleEnvironment env, EventArgs args)
    {
        if (args is not BattleCommandEventArgs battleArgs) return;
        
        HandleEvent(holder, env, battleArgs);
    }

    protected IEnumerable<RelationContext> GetRelation(EntityUid holder, BattleEnvironment env,
        IBattleCommandEventArgs args)
    {
        if (AffectedEntity < FunctionTarget.Allies)
        {
            return
            [
                new RelationContext
                {
                    Actor = holder,
                    Target = AffectedEntity switch
                    {
                        FunctionTarget.This => holder,
                        FunctionTarget.Sender => args.Sender,
                        FunctionTarget.Target => args.Target,
                        _ => throw new NotSupportedException(),
                    },
                },
            ];
        }

        return env.GetNeighbors(AffectedEntity switch
        {
            FunctionTarget.Allies => holder,
            FunctionTarget.Enemies => args.Target,
            _ => throw new NotSupportedException("Unknown FunctionTarget: " + AffectedEntity),
        }).Select(target => new RelationContext
        {
            Actor = holder,
            Target = target,
        });
    }

    protected abstract void HandleEvent(EntityUid holder, BattleEnvironment env, BattleCommandEventArgs args);
}

public class HealReactionFunction : CommandReactionFunction
{
    public required double Ratio { get; init; }
    public bool CanOverheal { get; init; }

    protected override void HandleEvent(EntityUid holder, BattleEnvironment env, BattleCommandEventArgs args)
    {
        if (args is not IDeltaEventArgs delta) return;

        // TODO (Nova): actually publish reaction commands
        var cmd = GetRelation(holder, env, delta).Select(target => new HealCommand
        {
            Amount = delta.Delta * Ratio,
            Relation = target,
            Data = new HealData(CanOverheal),
        });
    }
}

public class ExecuteLinkReactionFunction : CommandReactionFunction
{
    public required ImmutableArray<ShardId> Shards { get; init; }
    protected override void HandleEvent(EntityUid holder, BattleEnvironment env, BattleCommandEventArgs args)
    {
        // TODO (Nova)
        var result = GetRelation(holder, env, args).Select(target =>
            new Selections.PlayerTurnAction(Shards, target.Target));
    }
}

public class SequenceReactionFunction : ReactionFunction
{
    public required ImmutableArray<ReactionFunction> Terms { get; init; }
    public int Repeat { get; init; } = 1;

    public override void HandleEvent(EntityUid holder, BattleEnvironment env, EventArgs args)
    {
        for (var i = 0; i < Repeat; i++)
        {
            foreach (var function in Terms)
            {
                function.HandleEvent(holder, env, args);
            }
        }
    }
}

public class ModifyCounterReactionFunction : ReactionFunction
{
    public required CounterName Counter { get; init; }
    public required CounterOperation Operation { get; init; }

    public override void HandleEvent(EntityUid holder, BattleEnvironment env, EventArgs args)
    {
        if (env.GetCounter(holder, Counter) is not { } counter) return;

        switch (Operation)
        {
            case CounterOperation.Increment:
                counter.TryIncrement();
                break;
            case CounterOperation.Decrement:
                counter.TryDecrement();
                break;
            case CounterOperation.Reset:
                counter.Reset();
                break;
            default:
                throw new InvalidOperationException("Unknown operation: " + Operation);
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter<CounterOperation>))]
    public enum CounterOperation
    {
        [JsonStringEnumMemberName("increment")]
        Increment,
        [JsonStringEnumMemberName("decrement")]
        Decrement,
        [JsonStringEnumMemberName("reset")]
        Reset,
    }
}