using System.Collections.Generic;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.EffectSystem;

namespace FrogBattleV4.Core.CharacterSystem;

public interface ICharacter : ITargetable, ISupportsEffects, IHasAbilities
{
    IReadOnlyDictionary<string, double> BaseStats { get; }
    
    double GetStat(string stat, ICharacter target = null);
}