using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.Calculation;

public static class ModifierPipeline
{
    public static double Calculate(this Query query, BattleEnvironment env)
    {
        return query switch
        {
            StaticQuery s => s.CalculateStatic(env),
            RelationQuery m => m.CalculateMutation(env),
            _ => throw new System.NotSupportedException("Query type not supported")
        };
    }
    private static double CalculateMutation(this RelationQuery query, BattleEnvironment env)
    {
        // (source.self + target.other) + (target.self + source.other)
        var actor = query.Context.Actor;
        var target = query.Context.Target;
        var actorMods = (IModifierProvider)null!;
        var targetMods = (IModifierProvider)null!;
        var totalValue = query.BaseValue;
        // kinda like this...
        totalValue = (actorMods.GetContributingModifiers(query,
                           new ModifierContext(actor, actor)) +
                       targetMods.GetContributingModifiers(query,
                           new ModifierContext(target, actor)))
            .ApplyTo(totalValue);
        totalValue = (targetMods.GetContributingModifiers(query,
                           new ModifierContext(target, target)) +
                       actorMods.GetContributingModifiers(query,
                           new ModifierContext(actor, target)))
            .ApplyTo(totalValue);
        return totalValue;
    }

    private static double CalculateStatic(this StaticQuery query, BattleEnvironment env)
    {
        var subject = query.Subject;
        var reference = query.Reference;
        var actorMods = (IModifierProvider)null!;

        var totalStack = actorMods.GetContributingModifiers(query,
            new ModifierContext(subject, subject));

        if (reference.HasValue)
        {
            var targetMods = (IModifierProvider)null!;
            totalStack += targetMods.GetContributingModifiers(query,
                new ModifierContext(reference.Value, subject));
        }

        return totalStack.ApplyTo(query.BaseValue);
    }

    private static double ApplyModifiers(RelationQuery query, IModifierProvider actorMods, IModifierProvider targetMods)
    {
        var actor = query.Context.Actor;
        var target = query.Context.Target;
        var totalValue = query.BaseValue;
        totalValue = (actorMods.GetContributingModifiers(query,
                          new ModifierContext(actor, actor)) +
                      targetMods.GetContributingModifiers(query,
                          new ModifierContext(target, actor)))
            .ApplyTo(totalValue);
        totalValue = (targetMods.GetContributingModifiers(query,
                          new ModifierContext(target, target)) +
                      actorMods.GetContributingModifiers(query,
                          new ModifierContext(actor, target)))
            .ApplyTo(totalValue);
        return totalValue;
    }
}