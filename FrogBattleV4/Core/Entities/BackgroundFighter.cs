using System.Collections.Frozen;
using System.Collections.Generic;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Effects.Modifiers;
using FrogBattleV4.Core.Effects.StatusEffects;

namespace FrogBattleV4.Core.Entities;

// Think "Ethereal support character that exists in your team,
// but not on the field, and can just attack at will"
public class BackgroundFighter : IBattleMember
{
    public required string Name { get; set; }

    public Team AlliedTeam { get; set; }

    private FrozenDictionary<StatId, double> BaseStats { get; set; } = Registry.BaseCharacterStats;
    private List<StatusEffectInstance> StatusEffects { get; } = [];

    public double GetStat(StatQuery query)
    {
        return BaseStats[query.Stat];
    }

    public IEnumerable<IModifierProvider> GetAllModifiers(ModifierContext ctx)
    {
        return StatusEffects;
    }
}