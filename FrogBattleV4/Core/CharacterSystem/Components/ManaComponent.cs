namespace FrogBattleV4.Core.CharacterSystem.Components;

public class ManaComponent(ICharacter owner) : PoolComponent(owner)
{
    public override string Id => nameof(Pools.Mana);
}