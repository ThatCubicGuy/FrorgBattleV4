using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.BattleSystem;

public struct BattleContext
{
    public ICharacter ActiveCharacter { get; init; }
    public List<ICharacter> Allies { get; init; }
    public List<ICharacter> Enemies { get; init; }
    public uint TurnNumber { get; init; }
    public IOrderedEnumerable<ICharacter> ActionOrder { get; init; }
}