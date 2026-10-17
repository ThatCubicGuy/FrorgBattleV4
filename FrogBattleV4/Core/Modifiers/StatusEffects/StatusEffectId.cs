namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public readonly record struct StatusEffectId(string Id)
{
    public override string ToString() => Id;
}