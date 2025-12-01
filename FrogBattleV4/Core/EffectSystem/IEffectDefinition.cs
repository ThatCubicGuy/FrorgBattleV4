using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem;

public interface IEffectDefinition
{
    string Id { get; }
    List<IModifierComponent> Modifiers { get; }
}