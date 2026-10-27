using System.Collections.Frozen;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core.Registries;

public class FighterRegistry(FrozenDictionary<FighterId, Fighter> items) : Registry<FighterId, Fighter>(items)
{
    public static readonly FighterRegistry Global = RegistryLoader.CreateFighters("frorgbattle").Load();
}