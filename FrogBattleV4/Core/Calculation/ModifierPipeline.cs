using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.Calculation;

[Pure]
public static class ModifierPipeline
{
    public static double CalculateStatic(this AttributeQuery query, IEnumerable<IModifierProvider> modifiers)
    {
        var subject = query.Subject;

        var totalStack = modifiers.GetContributingModifiers(query, null!/*TODO*/,
            new ModifierContext(subject, subject));

        return totalStack.ApplyTo(0);
    }

    /// <summary>
    /// Calculates the total value of a given query and returns it as a double.
    /// </summary>
    /// <param name="query">Query to calculate.</param>
    /// <param name="env">BattleEnvironment to calculate in.</param>
    /// <returns>The final calculated value for the given query.</returns>
    /// <exception cref="System.NotSupportedException">The given query type is unsupported.</exception>
    [Pure]
    public static double Calculate(this IQuery query, BattleEnvironment env)
    {
        return query switch
        {
            AttributeQuery q => q.CalculateAttribute(env),
            CommandQuery q => q.CalculateCommand(env),
            _ => throw new System.NotSupportedException($"Query {query.GetType().FullName} not supported.")
        };
    }

    [Pure]
    private static double CalculateAttribute(this AttributeQuery query, BattleEnvironment env)
    {
        var subject = query.Subject;
        var reference = query.Reference;
        var actorMods = env.GetModifiers(subject).Concat(env.SubjectiveModifiers);

        var totalStack = actorMods.GetContributingModifiers(query, env,
            new ModifierContext(subject, subject));

        if (reference.HasValue)
        {
            var targetMods = env.GetModifiers(reference.Value);
            totalStack += targetMods.GetContributingModifiers(query, env,
                new ModifierContext(reference.Value, subject));
        }

        // Here's the fun part:
        return totalStack.ApplyTo(0);
        // Stats as a separate value do not need to exist. In fact, they really mess with our systems,
        // because not all battle members would have a giant dictionary of stats.
        // Thus, the base stats for characters are just going to be BaseAddValue modifiers for that stat.
        // That's it. Nothing special.
    }

    [Pure]
    private static double CalculateCommand(this CommandQuery query, BattleEnvironment env)
    {
        // (source.self + target.other) + (target.self + source.other)
        var actor = query.Context.Actor;
        var target = query.Context.Target;
        var actorMods = env.GetModifiers(actor).Concat(env.SubjectiveModifiers).ToArray();
        var targetMods = env.GetModifiers(target).ToArray();
        var totalValue = query.BaseValue;
        // kinda like this...
        totalValue = (actorMods.GetContributingModifiers(query, env,
                          new ModifierContext(actor, actor)) +
                      targetMods.GetContributingModifiers(query, env,
                          new ModifierContext(target, actor)))
            .ApplyTo(totalValue);
        // modifiers get applied between incoming and outgoing
        totalValue = (targetMods.GetContributingModifiers(query, env,
                          new ModifierContext(target, target)) +
                      actorMods.GetContributingModifiers(query, env,
                          new ModifierContext(actor, target)))
            .ApplyTo(totalValue);
        return totalValue;
    }
}