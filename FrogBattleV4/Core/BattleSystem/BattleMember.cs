using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.CharacterSystem.Pools;
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.BattleSystem;

public abstract class BattleMember : ITakesTurns, IHasPools, ISupportsEffects
{
    protected readonly Dictionary<string, IPoolComponent> _pools = new();

    public event EventHandler<StatusEffectApplicationContext> EffectApplySuccess;
    public event EventHandler<StatusEffectApplicationContext> EffectApplyFailure;
    public event EventHandler<StatusEffectRemovalContext> EffectRemoveSuccess;
    public event EventHandler<StatusEffectRemovalContext> EffectRemoveFailure;

    [NotNull] public string Name { get; protected set; }
    [NotNull] public IEnumerable<IAction> Turns { get; protected set; }
    [NotNull] public IEnumerable<IDamageable> Parts { get; protected set; } = [];
    [NotNull] public IReadOnlyDictionary<string, IPoolComponent> Pools => _pools;
    [NotNull] public IEnumerable<IModifierComponent> AttachedEffects { get; } = [];

    public double GetStat(string stat, IBattleMember target = null)
    {
        throw new NotImplementedException();
    }

    public bool ApplyEffect(StatusEffectApplicationContext ctx)
    {
        throw new NotImplementedException();
    }

    public bool RemoveEffect(StatusEffectRemovalContext ctx)
    {
        throw new NotImplementedException();
    }
}