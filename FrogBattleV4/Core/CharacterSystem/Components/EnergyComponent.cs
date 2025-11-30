namespace FrogBattleV4.Core.CharacterSystem.Components;

public class EnergyComponent(ICharacter owner) : PoolComponent(owner)
{
    public override string Id => nameof(Pools.Energy);
}