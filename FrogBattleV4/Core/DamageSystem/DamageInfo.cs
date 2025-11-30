namespace FrogBattleV4.Core.DamageSystem;

public record DamageInfo(DamageTypes DamageType, double DefPen, double TypeResPen);

public enum DamageTypes
{
    True,
    Blunt,
    Slash,
    Pierce,
    Bullet,
    Blast,
    Magic
}