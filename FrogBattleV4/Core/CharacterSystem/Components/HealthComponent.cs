namespace FrogBattleV4.Core.CharacterSystem.Components;

public class HealthComponent(ICharacter owner) : PoolComponent(owner)
{
    public override string Id => nameof(Pools.Hp);
}