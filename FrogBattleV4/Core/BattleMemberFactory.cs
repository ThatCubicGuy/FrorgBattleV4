using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.BattleSystem.Actions;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Calculation.Pools;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.EffectSystem.Components;
using FrogBattleV4.Core.EffectSystem.Modifiers;
using FrogBattleV4.Core.EffectSystem.PassiveEffects;

namespace FrogBattleV4.Core;

public static class BattleMemberFactory
{
    public static CharacterBuilder CharacterWith => new();

    public record struct CreateCharacterOptions(
        string Name,
        IReadOnlyDictionary<StatId, double> BaseStatOverrides,
        PoolComponent[] Pools,
        AbilityDefinition[] Abilities,
        DamageMutModifier[] NormalModifiers,
        DamageMutModifier[] HeadshotModifiers,
        PassiveEffectDefinition[] Passives,
        IAction[] Turns);

    public sealed class CharacterBuilder
    {
        private CreateCharacterOptions _createOptions;

        public CharacterBuilder Name([NotNull] string name)
        {
            _createOptions.Name = name;
            return this;
        }

        public CharacterBuilder BaseStats([NotNull] IReadOnlyDictionary<StatId, double> stats)
        {
            _createOptions.BaseStatOverrides = stats;
            return this;
        }

        public CharacterBuilder Pools([NotNull] params PoolComponent[] pools)
        {
            _createOptions.Pools = pools;
            return this;
        }

        public CharacterBuilder Abilities([NotNull] params AbilityDefinition[] abilities)
        {
            _createOptions.Abilities = abilities;
            return this;
        }

        public CharacterBuilder HeadshotModifiers([NotNull] params DamageMutModifier[] mods)
        {
            _createOptions.HeadshotModifiers = mods;
            return this;
        }

        public CharacterBuilder NormalModifiers([NotNull] params DamageMutModifier[] mods)
        {
            _createOptions.NormalModifiers = mods;
            return this;
        }

        public CharacterBuilder Passives([NotNull] params PassiveEffectDefinition[] effects)
        {
            _createOptions.Passives = effects;
            return this;
        }

        public IBattleMember Build()
        {
            var stats = new Dictionary<StatId, double>(Registry.BaseCharacterStats);
            foreach (var stat in _createOptions.BaseStatOverrides)
            {
                stats[stat.Key] = stat.Value;
            }

            var character = new BattleMember
            {
                Name = _createOptions.Name,
                BaseStats = stats.ToFrozenDictionary(),
                Hitbox = new HumanoidHitbox
                {
                    Floating = false,
                    HeadshotModifiers = _createOptions.HeadshotModifiers.Select(dmm =>
                        new DamageMutModifier
                        {
                            Type = dmm.Type,
                            Source = dmm.Source,
                            CritOnly = dmm.CritOnly,
                            ModifierStack = dmm.ModifierStack,
                            Direction = CalcDirection.Self,
                            MutDirection = MutModifierDirection.Incoming,
                        }
                    ),
                    NormalModifiers = _createOptions.NormalModifiers.Select(dmm =>
                        new DamageMutModifier
                        {
                            Type = dmm.Type,
                            Source = dmm.Source,
                            CritOnly = dmm.CritOnly,
                            ModifierStack = dmm.ModifierStack,
                            Direction = CalcDirection.Self,
                            MutDirection = MutModifierDirection.Incoming,
                        }
                    ),
                },
                Turns = _createOptions.Turns,
            };

            // TODO: ok i kinda fricked up u can't exactly have add requests
            // how in the world do i make behaviour conditional
            // am i stupid
            // [yes]
            IEnumerable<CharacterPoolComponent> basePools =
            [
                new(character)
                {
                    Id = PoolId.Hp,
                    MaxValue = null,
                }
            ];
            foreach (var pool in basePools.Concat(_createOptions.Pools))
            {
                // the plan was to not add the exact reference given but honestly im not sure how else
                // cuz again there's different behaviours (CharacterPoolComponent for instance)
                // i might need to do definition-instance separation here as well cuz of the
                // state vs behaviour discrepancy
                character.Pools.Add(pool);
            }

            foreach (var passive in _createOptions.Passives)
            {
                character.Effects.AddPassive(passive);
            }

            return character;
        }
    }
}