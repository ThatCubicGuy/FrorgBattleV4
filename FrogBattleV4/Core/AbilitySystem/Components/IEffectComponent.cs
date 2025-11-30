namespace FrogBattleV4.Core.AbilitySystem.Components;

public interface IEffectComponent : IAbilityComponent
{
    void Apply(AbilityContext ctx);
}