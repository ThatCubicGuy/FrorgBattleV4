namespace FrogBattleV4.Core.AbilitySystem.Components;

public interface ICostComponent : IAbilityComponent
{
    void Tax(AbilityContext ctx);
}