using System.Collections.Generic;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.BattleSystem;

public struct BattleContext
{
    public ICharacter ActiveCharacter { get; init; }
    public List<ICharacter> Allies { get; init; }
    public List<ICharacter> Enemies { get; init; }
}