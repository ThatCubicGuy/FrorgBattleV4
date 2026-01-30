#nullable enable
using System.Collections.Generic;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.CharacterSystem;

/// <summary>
/// Targetable entities can be individually selected and hit by attacks.
/// </summary>
public interface ITargetable : IDamageable
{
    /// <summary>
    /// The actual battle member whom this targetable entity refers to.
    /// </summary>
    public IBattleMember Owner { get; }
}