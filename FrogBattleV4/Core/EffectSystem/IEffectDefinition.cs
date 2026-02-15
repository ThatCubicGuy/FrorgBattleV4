using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.Components;
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.EffectSystem;

public interface IEffectDefinition
{
    string Id { get; }
    IEnumerable<ModifierRule> Modifiers { get; }
}