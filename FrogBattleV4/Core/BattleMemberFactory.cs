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

public static class BattleMemberFactory
{
    public static CharacterBuilder CharacterWith => new();

    public record struct CreateCharacterOptions(
        string Name,
        IReadOnlyDictionary<StatId, double> BaseStatOverrides,
        IList<IPoolDefinition> Pools,
        IList<AbilityDefinition> Abilities,
        IList<PassiveEffectDefinition> Passives,
        IList<DamageMutModifier> NormalModifiers,
        IList<DamageMutModifier> HeadshotModifiers,
        IList<ScheduledAction> Turns);

    public sealed class CharacterBuilder
    {
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

        public CharacterBuilder BaseStatOverrides([NotNull] IReadOnlyDictionary<StatId, double> stats)
        {
            _createOptions.BaseStatOverrides = stats;
            return this;
        }

        public CharacterBuilder BaseStatOverrides([NotNull] params KeyValuePair<StatId, double>[] stats)
        {
            _createOptions.BaseStatOverrides = stats.ToDictionary();
            return this;
        }

        public CharacterBuilder BaseStatOverrides([NotNull] params (StatId, double)[] stats)
        {
            _createOptions.BaseStatOverrides = stats.ToDictionary();
            return this;
        }

        public CharacterBuilder Pools([NotNull] params IPoolDefinition[] pools)
        {
            _createOptions.Pools = pools;
            return this;
        }

        public CharacterBuilder Abilities([NotNull] params AbilityDefinition[] abilities)
        {
            _createOptions.Abilities = abilities;
            return this;
        }

        public CharacterBuilder HeadshotModifier(ModifierStack modStack, DamageType? type = null, DamageSource? source = null, bool critOnly = false)
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

        public CharacterBuilder NormalModifier(ModifierStack modStack, DamageType type, DamageSource source, bool critOnly)
        {
            _createOptions.NormalModifiers.Add(new DamageMutModifier
            {
                Type = type,
                Source = source,
                CritOnly = critOnly,
                ModifierStack = modStack,
                Direction = CalcDirection.Self,
                MutDirection = MutModifierDirection.Incoming,
            });
            return this;
        }

        public CharacterBuilder Passives([NotNull] params PassiveEffectDefinition[] effects)
        {
            _createOptions.Passives = effects;
            return this;
        }

        public CharacterBuilder Turns([NotNull] params ScheduledAction[] actions)
        {
            _createOptions.Turns = actions;
            return this;
        }

        public IBattleMember Build()
        {
            var stats = new Dictionary<StatId, double>(Registry.BaseCharacterStats);
            if (_createOptions.BaseStatOverrides is not null)
                foreach (var stat in _createOptions.BaseStatOverrides)
                {
                    stats[stat.Key] = stat.Value;
                }

            var character = new BattleMember
            {
                Name = _createOptions.Name,
                BaseStats = new StatContainer(stats),
                Hitbox = new HumanoidHitbox
                {
                    Floating = false,
                    HeadshotModifiers = _createOptions.HeadshotModifiers.ToFrozenSet(),
                    NormalModifiers = _createOptions.NormalModifiers.ToFrozenSet(),
                },
                Turns = _createOptions.Turns.ToFrozenSet(),
            };

            var buildCtx = new ModifierContext(character);

            Registry.ApplyBaseCharacterPools(character);

            foreach (var passive in _createOptions.Passives)
            {
                character.Effects.AddPassive(passive);
            }

            return character;
        }
    }
}