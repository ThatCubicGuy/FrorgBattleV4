using FrogBattleV4.Core.Combat;

namespace FrogBattleV4.Core.Entities;

public interface IFieldMember : IBattleMember
{
    // Being a physical entity on the field is what differentiates
    // an active field member from a battle member.
    // TODO: Hitbox and positioning!
    IHitbox Hitbox { get; }
}