#nullable enable
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.EffectSystem;

public struct EffectContext
{
    public ISupportsEffects Holder;
    public IBattleMember? Target;
    public IBattleMember? EffectSource;
}