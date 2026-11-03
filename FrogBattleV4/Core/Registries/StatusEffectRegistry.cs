using System.Collections.Frozen;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Registries;

public class StatusEffectRegistry(FrozenDictionary<StatusEffectId, StatusEffect> items)
    : Registry<StatusEffectId, StatusEffect>(items)
{
    public static readonly StatusEffectRegistry Global =
        new(DeferredRegistry.CreateStatusEffects("frorgbattle").LoadAll());
}