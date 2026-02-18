using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FrogBattleV4.Core.AbilitySystem;

public interface IHasAbilities
{
    /// <summary>
    /// Abilities that this battle member may use. These can do a lot of things.
    /// </summary>
    [NotNull] IEnumerable<AbilityDefinition> Abilities { get; }
}