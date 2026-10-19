namespace FrogBattleV4.Core.Combat.Commands;

public interface ICommandVisitor
{
    void Visit(DamageCommand command);
    void Visit(MutateCommand command);
    void Visit(ApplyEffectCommand command);
    void Visit(RemoveEffectCommand command);
    void Visit(AdvanceTurnCommand command);
    void Visit(QueueTurnCommand command);
}