#nullable enable
namespace FrogBattleV4.Core.DamageSystem;

public record DamageProperties(string? Type = null, double DefPen = 0, double TypeResPen = 0, bool CanCrit = true);

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