using System.Collections.Immutable;

namespace FrogBattleV4.Core.Selections;

public record SelectionResult<TResult>(ImmutableList<TResult> Choices);