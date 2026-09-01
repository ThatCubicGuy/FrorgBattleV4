using System.Diagnostics.CodeAnalysis;

namespace FrogBattleV4.Core.Calculation;

public interface IResultContext<out TResultTarget>
{
    [NotNull] TResultTarget ResultTarget { get; }
}