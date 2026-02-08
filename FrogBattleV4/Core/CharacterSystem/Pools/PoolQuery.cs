#nullable enable
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.CharacterSystem.Pools;

public struct PoolQuery
{
    public required PoolId PoolId { get; init; }
    public required ModifierDirection Direction { get; init; }
    public required PoolPropertyChannel Channel { get; init; }
}