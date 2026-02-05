using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.Components;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.EffectSystem;

public interface IEffectDefinition
{
    string Id { get; }
    IEnumerable<IModifierRule> Modifiers { get; }
}