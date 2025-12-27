using System.Collections.Generic;
using FrogBattleV4.Core.CharacterSystem.Components;

namespace FrogBattleV4.Core.CharacterSystem;

public interface IHasStats
{
    IReadOnlyDictionary<string, double> BaseStats { get; }
    IReadOnlyDictionary<string, IPoolComponent> Pools { get; }
}