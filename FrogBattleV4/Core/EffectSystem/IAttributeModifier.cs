using System.Collections.Generic;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem;

public interface IAttributeModifier
{
    List<IModifierComponent> Modifiers { get; init; }
    // double GetModifiedStat(string statName, ICharacter character);
}