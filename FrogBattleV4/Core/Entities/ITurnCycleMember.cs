namespace FrogBattleV4.Core.Entities;

public interface ITurnCycleMember
{
    // The abilities list isn't in any way something necessary
    // for battle members to act, to deal damage, etc. It is
    // provided for external decision makers to pick between.
    // This decision ALWAYS happens on a turn. Because I said so.
    // TODO: Turns AND Abilities

    /// <summary>
    /// Gets the next turn of this TurnCycleMember.
    /// </summary>
    /// <returns>A turn to be added to the action bar.</returns>
    Turn GetNextTurn();
}

public abstract class Turn
{
    // TODO !!!
}