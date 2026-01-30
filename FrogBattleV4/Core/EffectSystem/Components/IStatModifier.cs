using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.EffectSystem.Components;

public interface IStatModifier : IModifierComponent<StatCalcContext>
{
    string Stat { get; }
}