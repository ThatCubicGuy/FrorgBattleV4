#nullable enable
namespace FrogBattleV4.Core.DamageSystem;

public record DamageProperties(string Type, double DefPen = 0, double TypeResPen = 0);

public enum DamageType
{
    True,
    Blunt,
    Slash,
    Pierce,
    Bullet,
    Blast,
    Magic
}