using System.Collections.Generic;
using System.Collections.ObjectModel;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.Entities;

public abstract class BattleMember : GameEntity
{
    public required string Name { get; init; }
    public abstract IEnumerable<IModifierProvider> ProvideModifiers(BattleEnvironment env);
}

public class BattleMemberData
{
    public required BattleMember Member { get; init; }
    public required ReadOnlyDictionary<PoolId, double> PoolValues { get; init; }

    public IEnumerable<IModifierProvider> ProvideModifiers(BattleEnvironment env)
    {
        return Member.ProvideModifiers(env);
    }
}