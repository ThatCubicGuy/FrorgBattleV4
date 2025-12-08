#nullable enable
namespace FrogBattleV4.Core.DamageSystem;

public record DamageProperties(string? Type, double DefPen, double TypeResPen, bool CanCrit);

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