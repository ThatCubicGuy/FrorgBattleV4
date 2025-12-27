#nullable enable
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.CharacterSystem;

/// <summary>
/// Targetable entities can be individually selected and hit by attacks.
/// Left and right members allow treating them as a sort of linked list.
/// This is useful since position matters, but we can't guarantee order.
/// </summary>
public interface ITargetable
{
    /// <summary>
    /// The actual battle member whom this targetable entity refers to.
    /// </summary>
    public IBattleMember This { get; }
    /// <summary>
    /// The targetable entity directly on the left of this instance.
    /// </summary>
    public ITargetable? Left { get; }
    /// <summary>
    /// The targetable entity directly on the right of this instance.
    /// </summary>
    public ITargetable? Right { get; }
}