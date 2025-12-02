#nullable enable

using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem;

public interface IAttributeModifier
{
    List<IModifierComponent> Modifiers { get; }
}