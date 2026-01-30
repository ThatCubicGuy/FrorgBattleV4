namespace FrogBattleV4.Core.AbilitySystem.Components;

public interface ICostComponent : IAbilityComponent
{
    double GetBaseAmount(AbilityExecContext ctx);
}