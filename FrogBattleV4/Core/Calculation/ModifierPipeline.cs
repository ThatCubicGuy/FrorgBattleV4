using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.Calculation;

public static class ModifierPipeline
{
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
            StaticQuery s => s.CalculateStatic(env),
            DynamicQuery d => d.CalculateDynamic(env),
            _ => throw new System.NotSupportedException("Query type not supported")
        };
    }

    [Pure]
    private static double CalculateDynamic(this DynamicQuery query, BattleEnvironment env)
    {
        // (source.self + target.other) + (target.self + source.other)
        var actor = query.Context.Actor;
        var target = query.Context.Target;
        var actorMods = env.GetModifiers(actor);
        var targetMods = env.GetModifiers(target);
        var totalValue = query.BaseValue;
        // kinda like this...
        totalValue = (actorMods.GetContributingModifiers(query, env,
                          new ModifierContext(actor, actor)) +
                      targetMods.GetContributingModifiers(query, env,
                          new ModifierContext(target, actor)))
            .ApplyTo(totalValue);
        totalValue = (targetMods.GetContributingModifiers(query, env,
                          new ModifierContext(target, target)) +
                      actorMods.GetContributingModifiers(query, env,
                          new ModifierContext(actor, target)))
            .ApplyTo(totalValue);
        return totalValue;
    }

    [Pure]
    private static double CalculateStatic(this StaticQuery query, BattleEnvironment env)
    {
        var subject = query.Subject;
        var reference = query.Reference;
        var actorMods = env.GetModifiers(subject);

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
        // Thus, the base stats for characters are just going to be AddValue modifiers for that stat.
        // That's it. Nothing special. It works perfectly, and now we just need to expose
        // a GetModifiers() method or something for each battle member.
    }
}