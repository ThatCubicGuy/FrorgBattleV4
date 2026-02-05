#nullable enable

using FrogBattleV4.Core.Contexts;

namespace FrogBattleV4.Core.EffectSystem.PassiveEffects;

public interface IConditionComponent
{
    int GetContribution(EffectInfoContext ctx);
}