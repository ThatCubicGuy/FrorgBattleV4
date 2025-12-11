using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.AbilitySystem.Components;
using FrogBattleV4.Core.AbilitySystem.Components.Attacks;
using FrogBattleV4.Core.AbilitySystem.Components.Targeting;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Content.Characters;

public class Ayumi : Character
{
    public Ayumi()
    {
        Abilities =
        [
            new AyumiAbility1
            {
                Id = nameof(AyumiAbility1),
            },
            
        ];
    }
}

internal class AyumiAbility1 : AbilityDefinition
{
    public AyumiAbility1()
    {
        Targeting = new Single();
        Attacks =
        [
            new AttackComponent
            {
                Scalar = nameof(Stat.Atk),
                Ratio = 1.76,
                Properties = new DamageProperties
                {
                    Type = nameof(DamageType.Pierce),
                    CanCrit = true,
                    DefPen = 0.33,
                },
                HitRate = 0.9,
            }
        ];
    }
}