namespace FrogBattleV4.Core.Combat.Actions;

public interface IActionVisitor
{
    void Visit(DamageCommand command);
    void Visit(MutateCommand command);
    void Visit(ApplyEffectCommand command);
    void Visit(RemoveEffectCommand command);
    void Visit(AdvanceTurnCommand command);
    void Visit(QueueTurnCommand command);
}