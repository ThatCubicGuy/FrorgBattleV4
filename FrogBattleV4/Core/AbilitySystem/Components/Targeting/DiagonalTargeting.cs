using System.Collections.Generic;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class DiagonalTargeting(int radius) : ITargetingComponent
{
    public IEnumerable<TargetingContext> SelectTargets(AbilityExecContext ctx)
    {
        // TODO: Make diagonal stuff lol
        throw new System.NotImplementedException();
    }
}