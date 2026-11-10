using System.Text.Json.Serialization;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers;

// Stat modifiers inherit directly from 
public class StatModifier : IModifierRule
{
    [JsonPropertyName("values")]
    public required ModifierStack ModifierValues { get; init; }

    /// <summary>
    /// Determines whether this modifier applies to the holder or the target.
    /// </summary>
    [JsonPropertyName("entity")]
    public AffectedSide AffectedSide { get; init; } = AffectedSide.This;

    public required StatName Stat { get; init; }

    bool IModifierRule.AppliesTo(IQuery query, in ModifierContext ctx)
    {
        return MatchesRelationship(query, ctx) && Applies(query);
    }

    private bool MatchesRelationship(IQuery query, in ModifierContext ctx)
    {
        var subject = query.Main;
        var reference = query.Other;
        return AffectedSide switch
        {
            AffectedSide.This => ctx.Holder == subject,
            AffectedSide.Other => ctx.Holder == reference,
            _ => throw new System.NotSupportedException($"{AffectedSide} not supported"),
        } && ctx.Subject == subject;
    }

    private bool Applies(IQuery query)
    {
        return (query as StatQuery)?.Stat == Stat || Stat switch
        {
            _ => false,
        };
    }
}