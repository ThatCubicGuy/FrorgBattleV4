using System;
using System.Text.Json.Serialization;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Reactions.Functions;

public class ModifyCounterReactionFunction : ReactionFunction
{
    public required CounterName Counter { get; init; }
    public required CounterOperation Operation { get; init; }

    public override void HandleEvent(EntityUid holder, EventArgs args, BattleEnvironment env, CommandBuilder builder)
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