namespace FrogBattleV4.Core.EffectSystem.ActiveEffects;

public class ActiveEffectInstance
{
    public ActiveEffectDefinition Definition { get; init; }
    public uint Turns { get; set; }
    public uint Stacks { get; set; }
}