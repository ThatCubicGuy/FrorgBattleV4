using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.Combat.Actions;

namespace FrogBattleV4.Core.Combat;

public interface ITakesTurns
{
    /// <summary>
    /// Turns that this IBattleMember may take during the battle.
    /// They appear on the actionbar.
    /// </summary>
    [NotNull] IEnumerable<IScheduledAction> Turns { get; }
}