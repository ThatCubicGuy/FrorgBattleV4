using System.Linq;
using FrogBattleV4.Core.Combat.Commands;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core;

public sealed partial class BattleManager
{
    private class CommandResolver(BattleManager manager) : ICommandVisitor
    {
        public void Visit(DamageCommand command)
        {
            var (actor, target) = command.Relation;
            var damageData = command.Data;
            var damage = manager._rng.Chance(command.CriticalChance)
                ? command.CriticalAmount
                : command.NormalAmount;

            manager.Entities[target].TakeDamage(damage, damageData);

            manager.OnDamageTaken(new DamageTakenEventArgs
            {
                Sender = actor,
                Target = target,
                Amount = damage,
                Data = damageData,
            });
        }

        public void Visit(HealCommand command)
        {
            var (actor, target) = command.Relation;
            var healData = command.Data;
            var healing = command.Amount;

            manager.Entities[target].TakeHealing(healing, healData);

            manager.OnHealingReceived(new HealingReceivedEventArgs
            {
                Sender = actor,
                Target = target,
                Amount = healing,
                Data = healData,
            });
        }

        public void Visit(MutateCommand command)
        {
            var (actor, target) = command.Relation;
            var data = command.Data;
            var oldValue = manager.Entities[target].Pools[data.TargetPool].Value;

            manager.Entities[target].Pools[data.TargetPool].Value += command.TotalAmount;

            manager.OnPoolMutated(new PoolMutatedEventArgs
            {
                Sender = actor,
                Target = target,
                Pool = data.TargetPool,
                OldValue = oldValue,
                NewValue = manager.Entities[target].Pools[data.TargetPool].Value,
            });
        }

        public void Visit(ApplyEffectCommand command)
        {
            var (actor, target) = command.Relation;
            var data = command.Data;

            if (!manager._rng.Chance(command.ApplicationChance))
            {
                manager.OnEffectResisted(new EffectResistedEventArgs
                {
                    Sender = actor,
                    Target = target,
                });
                return;
            }

            var definition = manager._statusEffectRegistry.Get(data.Effect);

            manager.Entities[target].AddStatusEffect(new StatusEffectInstance
            {
                Applicant = actor,
                Holder = target,
                Definition = definition,
                Stacks = data.AddedStacks,
                Turns = data.InitialTurns,
            });

            manager.OnEffectApplied(new EffectAppliedEventArgs
            {
                Sender = actor,
                Target = target,
                Id = definition.Id,
                Stacks = data.AddedStacks,
                Turns = data.InitialTurns,
            });
        }

        public void Visit(RemoveEffectCommand command)
        {
            var (actor, target) = command.Relation;
            var data = command.Data;

            var effectsToRemove = manager.Entities[target].StatusEffects
                .Where(effect => data.Predicate(effect) &&
                                 !effect.Definition.Properties.HasFlag(EffectFlags.Unremovable))
                // Use ToList() to avoid CollectionChanged exceptions at runtime
                .ToList();

            foreach (var instance in effectsToRemove)
            {
                if (!manager.Entities[target].TryGetStatusEffect(instance.Definition.Id, out var effect)) continue;
                if (instance.Definition.Properties.HasFlag(EffectFlags.RemoveStack))
                {
                    effect.Stacks -= data.RemovedStacks;
                }
                else effect.Stacks = 0;

                if (effect.ShouldRemove() && manager.Entities[target].RemoveStatusEffect(instance.Definition.Id))
                {
                    manager.OnEffectRemoved(new EffectRemovedEventArgs
                    {
                        Sender = actor,
                        Target = target,
                        Id = instance.Definition.Id,
                        TurnsLeft = instance.Turns,
                        Stacks = instance.Stacks,
                    });
                }
            }
        }

        public void Visit(AdvanceTurnCommand command)
        {
            var (actor, target) = command.Relation;
            var data = command.Data;

            manager.Scheduler.AdvanceEntity(target, data.AdvancePercent);

            manager.OnTurnAdvanced(new TurnAdvancedEventArgs
            {
                Sender = actor,
                Target = target,
                AdvanceAmount = data.AdvancePercent,
            });
        }

        public void Visit(QueueTurnCommand command)
        {
            var (actor, target) = command.Relation;
            var data = command.Data;

            manager.Scheduler.Schedule(target, data.BaseActionValue);

            manager.OnTurnQueued(new TurnQueuedEventArgs
            {
                Sender = actor,
                Target = target,
            });
        }
    }
}