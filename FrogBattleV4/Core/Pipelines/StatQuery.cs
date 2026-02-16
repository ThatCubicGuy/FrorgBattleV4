using System.Diagnostics.CodeAnalysis;

namespace FrogBattleV4.Core.Pipelines;

[method: SetsRequiredMembers]
public readonly struct StatQuery(StatId stat)
{
    public required StatId Stat { get; init; } = stat;
}