using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Modifiers;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Entities;

public abstract class FighterBase : BattleMember,
    ITurnCycleMember,
    IModifierProvider
{
    private readonly List<StatusEffectInstance> _statusEffects = [];
    public FrozenDictionary<StatId, double> BaseStats { get; init; } = Registry.BaseCharacterStats;
    public ReadOnlyCollection<StatusEffectInstance> StatusEffects => _statusEffects.AsReadOnly();

    public double GetStat(StatId stat, EntityUid target, BattleEnvironment env)
    {
        return new StatQuery
        {
            Stat = stat,
            Subject = Id,
            Reference = target,
        }.Calculate(env);
    }

    public Turn GetNextTurn()
    {
        throw new System.NotImplementedException();
    }

    public ModifierStack GetContributingModifiers(IQuery query, BattleEnvironment env, ModifierContext ctx)
    {
        return _statusEffects.Aggregate(new ModifierStack(),
            (stack, modifier) => stack + modifier.GetContributingModifiers(query, env, ctx));
    }
}