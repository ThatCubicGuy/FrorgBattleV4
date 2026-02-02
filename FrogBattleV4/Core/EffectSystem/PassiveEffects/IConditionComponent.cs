#nullable enable
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.EffectSystem.PassiveEffects;

public interface IConditionComponent
{
    int GetContribution(EffectContext ctx);
}