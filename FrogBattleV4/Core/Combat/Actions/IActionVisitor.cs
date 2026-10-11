namespace FrogBattleV4.Core.Combat.Actions;

public interface IActionVisitor
{
    void Visit(Damage action);
    void Visit(Mutate action);
    void Visit(ApplyEffect action);
    void Visit(RemoveEffect action);
    void Visit(AdvanceTurn action);
    void Visit(QueueTurn action);
}