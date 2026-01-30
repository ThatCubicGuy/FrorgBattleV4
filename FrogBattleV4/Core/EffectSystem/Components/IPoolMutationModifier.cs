using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.CharacterSystem.Components;

namespace FrogBattleV4.Core.EffectSystem.Components;

public interface IPoolMutationModifier : IModifierComponent<PoolCalcContext>
{
    PoolModType Channel { get; }
    string PoolId { get; }
}