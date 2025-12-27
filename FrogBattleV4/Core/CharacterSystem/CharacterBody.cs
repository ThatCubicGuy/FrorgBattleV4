#nullable enable
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.CharacterSystem;

/// <summary>
/// The targetable component of any character.
/// </summary>
/// <param name="owner">The character this instance belongs to.</param>
public class CharacterBody(ICharacter owner) : ITargetable
{
    public IBattleMember This { get; } = owner;
    public ITargetable? Left { get; init; }
    public ITargetable? Right { get; init; }
}