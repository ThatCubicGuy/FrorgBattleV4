using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Calculation.Pools;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Combat.Actions;
using FrogBattleV4.Core.Effects.Components;
using FrogBattleV4.Core.Effects.Modifiers;
using FrogBattleV4.Core.Effects.PassiveEffects;

namespace FrogBattleV4.Core;

// Factory methods separated into a different file
public partial class BattleMember
{
    public static CharacterBuilder CharacterWith => new();
    public sealed class CharacterBuilder
    {
        private record struct CreateCharacterOptions(
            string Name,
            Dictionary<StatId, double> BaseStatOverrides,
            List<IPoolDefinition> Pools,
            List<AbilityDefinition> Abilities,
            List<PassiveEffectDefinition> Passives,
            List<DamageMutModifier> NormalModifiers,
            List<DamageMutModifier> HeadshotModifiers,
            List<ScheduledAction> Turns);

        private CreateCharacterOptions _createOptions = new()
        {
            Name = "Character",
            HeadshotModifiers = [],
            NormalModifiers = [],
            Pools = [],
            Abilities = [],
            Passives = [],
            Turns = [],
        };

        public CharacterBuilder Name([NotNull] string name)
        {
            _createOptions.Name = name;
            return this;
        }

        public CharacterBuilder BaseStat(StatId statId, double value)
        {
            _createOptions.BaseStatOverrides.Add(statId, value);
            return this;
        }

        public CharacterBuilder Pools([NotNull] params IPoolDefinition[] pools)
        {
            _createOptions.Pools.AddRange(pools);
            return this;
        }

        public CharacterBuilder Abilities([NotNull] params AbilityDefinition[] abilities)
        {
            _createOptions.Abilities.AddRange(abilities);
            return this;
        }

        public CharacterBuilder HeadshotModifier(ModifierStack modStack, DamageType? type = null,
            DamageSource? source = null, bool critOnly = false)
        {
            _createOptions.HeadshotModifiers.Add(new DamageMutModifier
            {
                Type = type ?? DamageType.None,
                Source = source ?? DamageSource.None,
                CritOnly = critOnly,
                ModifierStack = modStack,
                Direction = CalcDirection.Self,
                MutDirection = MutModifierDirection.Incoming,
            });
            return this;
        }

        public CharacterBuilder NormalModifier(ModifierStack modStack, DamageType? type = null,
            DamageSource? source = null, bool critOnly = false)
        {
            _createOptions.NormalModifiers.Add(new DamageMutModifier
            {
                Type = type ?? DamageType.None,
                Source = source ?? DamageSource.None,
                CritOnly = critOnly,
                ModifierStack = modStack,
                Direction = CalcDirection.Self,
                MutDirection = MutModifierDirection.Incoming,
            });
            return this;
        }

        public CharacterBuilder Passives([NotNull] params PassiveEffectDefinition[] effects)
        {
            _createOptions.Passives = effects.ToList();
            return this;
        }

        public CharacterBuilder Turns([NotNull] params ScheduledAction[] actions)
        {
            _createOptions.Turns = actions.ToList();
            return this;
        }

        public IBattleMember Build()
        {
            var stats = new Dictionary<StatId, double>(Registry.BaseCharacterStats);
            if (_createOptions.BaseStatOverrides is not null)
            {
                foreach (var stat in _createOptions.BaseStatOverrides)
                {
                    stats[stat.Key] = stat.Value;
                }
            }

            var character = new BattleMember(_createOptions.Name)
            {
                Hitbox = new HumanoidHitbox
                {
                    Floating = false,
                    HeadshotModifiers = _createOptions.HeadshotModifiers.ToFrozenSet(),
                    NormalModifiers = _createOptions.NormalModifiers.ToFrozenSet(),
                },
                Turns = _createOptions.Turns.ToFrozenSet(),
                Components =
                {
                    new AbilityContainer(_createOptions.Abilities),
                    new StatContainer(stats),
                    new PoolContainer(),
                    new EffectContainer(),
                },
            };

            // Generate component caches
            character.BuildCaches();

            // Add pools
            if (Registry.BaseCharacterPools.Values.Concat(_createOptions.Pools).Any(pd =>
                    !character.Pools.Add(new PoolInitContext
                    {
                        Definition = pd,
                        Target = character,
                    }))) throw new System.InvalidOperationException("Pool add failure");

            // Add passive effects
            foreach (var passive in _createOptions.Passives)
            {
                character.Effects.AddPassive(passive);
            }

            return character;
        }
    }
}