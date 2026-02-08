using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.BattleSystem.Actions;

namespace FrogBattleV4.Core.BattleSystem;

public interface ITakesTurns
{
    /// <summary>
    /// Turns that this IBattleMember may take during the battle.
    /// They appear on the actionbar.
    /// </summary>
    [NotNull] IEnumerable<IAction> Turns { get; }
}