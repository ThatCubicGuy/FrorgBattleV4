#nullable enable
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.Contexts;

public interface IActorContext
{
    BattleMember? Actor { get; }
}