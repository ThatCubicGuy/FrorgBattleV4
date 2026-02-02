using System.Diagnostics.CodeAnalysis;

namespace FrogBattleV4.Core.Contexts;

public interface IResultContext<out TResultTarget>
{
    [NotNull] TResultTarget ResultTarget { get; }
}