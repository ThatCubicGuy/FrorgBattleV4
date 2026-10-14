namespace FrogBattleV4.Core.Combat.Actions;

public interface IActionVisitor
{
    void Visit(DamageAction action);
    void Visit(MutateAction action);
    void Visit(ApplyEffectAction action);
    void Visit(RemoveEffectAction action);
    void Visit(AdvanceTurnAction action);
    void Visit(QueueTurnAction action);
}