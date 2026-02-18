using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FrogBattleV4.Core.BattleSystem;

public class Team([NotNull] params IBattleMember[] battleMembers)
{
    [NotNull] public IEnumerable<IBattleMember> Members { get; } = battleMembers.ToList();
}