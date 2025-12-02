using System.Linq;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.Extensions;

public static class ModifierExtensions
{
    public static bool IsBuff(this IStatModifier mod)
    {
        var side = mod.Stat switch
        {
            nameof(Stat.ManaCost) or nameof(Stat.MaxEnergy) => false,
            _ => true
        };
        return mod.Operation switch
        {
            ModifierOperation.AddValue => (mod.Amount > 0) == side,
            ModifierOperation.MultiplyBase or ModifierOperation.MultiplyTotal => (mod.Amount > 1) == side,
            _ => false
        };
    }
    public static string Format(this IStatModifier mod)
    {
        return mod.Operation switch
        {
            ModifierOperation.AddValue => $"{(mod.IsBuff() ? '+' : string.Empty)}{mod.Amount:N0} ",
            ModifierOperation.MultiplyBase => $"{(mod.IsBuff() ? '+' : string.Empty)}{mod.Amount - 1:P0} ",
            ModifierOperation.MultiplyTotal => $"{mod.Amount:0.##}x ",
            _ => $"{mod.Amount:F1}"
        } + mod.Stat;
    }

    public static double GetModifiedStat(this IAttributeModifier mod, string statName, StatContext ctx)
    {
        // return mod.Modifiers.OfType<IStatModifier>().Where(x => x.Stat == statName).Sum(x => x)
        throw new System.NotImplementedException();
    }
}