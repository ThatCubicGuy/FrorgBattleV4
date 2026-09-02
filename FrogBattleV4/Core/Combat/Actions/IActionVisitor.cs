namespace FrogBattleV4.Core.Combat.Actions;

public interface IActionVisitor
{
    void Visit(ApplyEffect action);
    void Visit(Damage action);
    void Visit(Mutate action);
    void Visit(RemoveEffect action);
    void Visit(TurnAdvance action);
    void Visit(QueueTurn action);
}