namespace FrogBattleV4.Core.Combat;

public record EnqueueActionData(
    TurnDecision Decision,
    // ActionType determines the priority of this action among other actions.
    ActionType Type);

public enum ActionType
{
    Unspecified,
    FollowUp,
    SoulLink,
}