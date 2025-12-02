#nullable enable

using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem;

/// <summary>
/// An attribute modifier is a wrapper of various modifiers that is
/// attached to a fighter, and only makes sense in that context.
/// </summary>
public interface IAttributeModifier
{
    double GetModifiedAttribute(string attribute, StatContext ctx);
}