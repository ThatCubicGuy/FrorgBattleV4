namespace FrogBattleV4.Core.AbilitySystem.Components;

public interface IRequirementComponent
{
    bool Check(AbilityContext ctx);
}