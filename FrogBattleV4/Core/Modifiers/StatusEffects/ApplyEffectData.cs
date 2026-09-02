namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public record ApplyEffectData(
    StatusEffectDefinition Effect,
    int InitialTurns,
    int AddedStacks = 1);

public record EffectChanceResolution(double Chance, double Roll)
{
    public bool CanApply => Chance > Roll;
}