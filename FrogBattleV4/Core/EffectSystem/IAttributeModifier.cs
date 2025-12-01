#nullable enable
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.EffectSystem;

public interface IAttributeModifier
{
    double GetModifiedStat(string statName, double currentValue, EffectContext ctx);
}