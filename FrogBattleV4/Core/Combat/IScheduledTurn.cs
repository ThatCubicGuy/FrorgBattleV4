namespace FrogBattleV4.Core.Combat;

public interface IScheduledTurn
{
    double BaseActionValue { get; }
    EntityUid Actor { get; }
}