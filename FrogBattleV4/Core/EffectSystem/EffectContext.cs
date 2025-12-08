#nullable enable
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.EffectSystem;

public struct StatContext
{
    public string Stat;
    public ICharacter Holder;
    public ICharacter? Target;
    public ICharacter? EffectSource;
}