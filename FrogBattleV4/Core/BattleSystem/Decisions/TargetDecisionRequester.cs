using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.BattleSystem.Decisions;

public record TargetDecisionRequester(ICharacter Owner) : IDecisionRequester<ITargetable>
{
    public IEnumerable<ITargetable> GetOptions(BattleContext ctx) => ctx.Enemies.SelectMany(x => x.Parts);
}