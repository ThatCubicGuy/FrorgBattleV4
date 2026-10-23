using System;
using System.Linq;
using FrogBattleV4.Core.Combat.Commands;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core;

public partial class BattleManager
{
    public event EventHandler<EntityUid>? EffectResisted;

    protected virtual void OnEffectResisted(EntityUid e)
    {
        EffectResisted?.Invoke(this, e);
    }

    private class CommandResolver : ICommandVisitor
    {
        private readonly Core.BattleManager _manager;

        public CommandResolver(Core.BattleManager manager)
        {
            _manager = manager;
        }

        public void Visit(DamageCommand command)
        {
            var (_, target) = command.Relation;
            var data = command.Data;

            _manager.Entities[target]
                .TakeDamage(_manager._rng.Chance(command.CriticalChance)
                        ? command.CriticalAmount
                        : command.NormalAmount,
                    data);
        }

        public void Visit(MutateCommand command)
        {
            var (_, target) = command.Relation;
            var data = command.Data;

            _manager.Entities[target].Pools[data.TargetPool].Value += command.TotalAmount;
        }

        public void Visit(ApplyEffectCommand command)
        {
            var (actor, target) = command.Relation;
            var data = command.Data;

            if (!_manager._rng.Chance(command.ApplicationChance))
            {
                _manager.OnEffectResisted(target);
                return;
            }

            _manager.Entities[target].AddStatusEffect(new StatusEffectInstance
            {
                Applicant = actor,
                Holder = target,
                Definition = data.Effect,
                Stacks = data.AddedStacks,
                Turns = data.InitialTurns
            });
        }

        public void Visit(RemoveEffectCommand command)
        {
            var (_, target) = command.Relation;
            var data = command.Data;

            foreach (var effect in _manager.Entities[target].StatusEffects.Where(data.Predicate))
            {
                if (effect.Definition.Properties.HasFlag(EffectFlags.Unremovable)) continue;

                if (effect.Definition.Properties.HasFlag(EffectFlags.RemoveStack))
                {
                    var eff = _manager.Entities[target].StatusEffects
                        .SingleOrDefault(sei => sei.Definition.Id == effect.Definition.Id);
                    if (eff != null) eff.Stacks -= data.RemovedStacks;
                }
                else
                {
                    _manager.Entities[target].RemoveStatusEffect(effect.Definition.Id);
                }
            }
        }

        public void Visit(AdvanceTurnCommand command)
        {
            var (_, target) = command.Relation;
            var data = command.Data;

            _manager.Scheduler.AdvanceEntity(target, data.AdvancePercent);
        }

        public void Visit(QueueTurnCommand command)
        {
            var (_, target) = command.Relation;
            var data = command.Data;

            _manager.Scheduler.Schedule(target, data.BaseActionValue);
        }
    }
}