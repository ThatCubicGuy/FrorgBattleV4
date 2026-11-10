using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FrogBattleV4.Core.Abilities.Reactions;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Entities;
using FrogBattleV4.Core.Registries;
using FrogBattleV4.Core.Decisions;

namespace FrogBattleV4.Core;

public sealed partial class BattleManager
{
    public static Builder GetBuilder()
    {
        return new Builder(FighterRegistry.Global, ShardRegistry.Global, StatusEffectRegistry.Global);
    }

    public class Builder
    {
        private readonly Dictionary<EntityUid, EntityInstance> _fighters = [];
        private readonly List<Team> _teams = [];
        private readonly FighterRegistry _fighterRegistry;
        private readonly ShardRegistry _shardRegistry;
        private readonly StatusEffectRegistry _statusEffectRegistry;

        internal Builder(FighterRegistry fighterRegistry,
            ShardRegistry shardRegistry,
            StatusEffectRegistry statusEffectRegistry)
        {
            _fighterRegistry = fighterRegistry;
            _shardRegistry = shardRegistry;
            _statusEffectRegistry = statusEffectRegistry;
        }

        /// <summary>
        /// Adds a new team to the builder with the given selection provider.
        /// </summary>
        /// <param name="provider">An object that will resolve selection requests for this team.</param>
        /// <returns>This builder instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> is null.</exception>
        public Builder AddTeam(IDecisionProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider);
            if (_teams.Count > 0 && !_teams[^1].GetAllMembers().Any())
            {
                throw new InvalidOperationException("Cannot add a team when the previous team contains no members.");
            }

            _teams.Add(new Team(provider));
            return this;
        }

        /// <summary>
        /// Adds a member to the last added team of this builder.
        /// </summary>
        /// <param name="member">Member to add to the team.</param>
        /// <returns>This builder instance.</returns>
        /// <exception cref="InvalidOperationException">No team was added yet.</exception>
        public Builder AddMember(FighterId member)
        {
            if (_teams.Count == 0)
            {
                throw new InvalidOperationException("Cannot add members when there are no teams.");
            }

            var definition = _fighterRegistry.Get(member);
            var team = _teams[^1];
            var instance = new EntityInstance(definition, team)
            {
                Owner = member,
                Team = team.Uid,
            };
            _fighters[instance.Uid] = instance;
            team.AddMember(instance.Uid);
            return this;
        }

        /// <summary>
        /// Adds multiple members to the last added team of this builder.
        /// </summary>
        /// <param name="members">Members to add to the team.</param>
        /// <returns>This builder instance.</returns>
        /// <exception cref="InvalidOperationException">No team was added yet.</exception>
        public Builder AddMembers(IEnumerable<FighterId> members)
        {
            if (_teams.Count == 0)
            {
                throw new InvalidOperationException("Cannot add members when there are no teams.");
            }

            foreach (var member in members)
            {
                AddMember(member);
            }

            return this;
        }

        public BattleManager Build()
        {
            if (_teams.Count < 2)
            {
                throw new InvalidOperationException("Cannot build a battle with less than two teams.");
            }

            if (!_teams[^1].GetAllMembers().Any())
            {
                throw new InvalidOperationException("Cannot build a battle when a team contains no members.");
            }

            var manager = new BattleManager(
                _fighters.ToImmutableDictionary(),
                _teams.ToImmutableDictionary(t => t.Uid),
                _fighterRegistry,
                _shardRegistry,
                _statusEffectRegistry);

            // Load reaction events
            var loader = new ReactionLoader(manager);
            foreach (var entity in _fighters.Values)
            {
                loader.CurrentEntity = entity.Uid;
                foreach (var reaction in _fighterRegistry.Get(entity.Owner).Reactions)
                {
                    reaction.Accept(loader);
                }
            }

            return manager;
        }

        private class ReactionLoader(BattleManager manager) : IReactionVisitor
        {
            public EntityUid CurrentEntity { get; set; }

            public void Visit(DamageTakenReaction reaction)
            {
                manager.DamageTaken += ReactionWrapper(reaction);
            }

            public void Visit(PoolMutatedReaction reaction)
            {
                manager.PoolMutated += ReactionWrapper(reaction);
            }

            public void Visit(EffectAppliedReaction reaction)
            {
                manager.EffectApplied += ReactionWrapper(reaction);
            }

            public void Visit(CounterTriggeredReaction reaction)
            {
                manager.Entities[CurrentEntity].Counters[reaction.Counter].TriggerValueReached +=
                    ReactionWrapper(reaction);
            }

            private EventHandler ReactionWrapper(Reaction reaction)
            {
                // Snapshot the current entity inside a local variable
                var entity = CurrentEntity;
                return (_, args) =>
                {
                    var env = manager.GetEnvironment();

                    foreach (var function in reaction.Functions)
                    {
                        function.HandleEvent(entity, args, env, manager._commandBuilder);
                    }
                };
            }

            // need generics here because TEventArgs of EventHandler<TEventArgs> is not contravariant
            private EventHandler<TEventArgs> ReactionWrapper<TEventArgs>(Reaction<TEventArgs> reaction)
                where TEventArgs : EventArgs
            {
                // Snapshot the current entity inside a local variable
                var entity = CurrentEntity;
                return (_, args) =>
                {
                    var env = manager.GetEnvironment();

                    if (!reaction.Matches(entity, env, args)) return;
                    foreach (var function in reaction.Functions)
                    {
                        function.HandleEvent(entity, args, env, manager._commandBuilder);
                    }
                };
            }
        }
    }
}