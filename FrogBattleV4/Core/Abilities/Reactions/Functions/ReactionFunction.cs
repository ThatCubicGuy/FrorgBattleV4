using System;
using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Abilities.Reactions.Functions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "function")]
[JsonDerivedType(typeof(HealReactionFunction), "heal")]
[JsonDerivedType(typeof(ExecuteLinkReactionFunction), "execute_link")]
[JsonDerivedType(typeof(SequenceReactionFunction), "sequence")]
[JsonDerivedType(typeof(ModifyCounterReactionFunction), "modify_counter")]
public abstract class ReactionFunction
{
    public abstract void HandleEvent(EntityUid holder, BattleEnvironment env, EventArgs args);
}