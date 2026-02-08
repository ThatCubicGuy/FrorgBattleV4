using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.AbilitySystem.Components.Attacks;
using FrogBattleV4.Core.AbilitySystem.Components.Targeting;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Content.Characters;

public class Ayumi : Character
{
    public Ayumi() : base("Ayumi")
    {
        Abilities =
        [
            new AyumiAbility1()
        ];
    }

    private class AyumiAbility1 : AbilityDefinition
    {
        [SetsRequiredMembers]
        // goofy ahh warning lol. it's right there !!!!
        // ReSharper disable once NotNullOrRequiredMemberIsNotInitialized
        public AyumiAbility1()
        {
            Id = nameof(AyumiAbility1);
            Targeting = new SingleTargeting();
            Attacks =
            [
                new AttackComponent
                {
                    Scalar = nameof(Stat.Atk),
                    Ratio = 1.76,
                    DamageProperties = new DamageProperties(nameof(DamageType.Pierce), 0.33),
                    HitRate = 0.9,
                    Targeting = new BlastTargeting(2),
                    AttackProperties = new AttackProperties(true)
                }
            ];
        }
    }
}