#nullable enable
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.CharacterSystem.Pools;

public struct PoolQuery
{
    public required string PoolId { get; init; }
    public required ModifierDirection Direction { get; init; }
    public required PoolPropertyChannel Channel { get; init; }
}