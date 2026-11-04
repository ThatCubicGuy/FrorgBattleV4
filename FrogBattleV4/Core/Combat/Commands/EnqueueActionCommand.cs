namespace FrogBattleV4.Core.Combat.Commands;

public class EnqueueActionCommand : BattleCommand<EnqueueActionData>
{
    public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
}