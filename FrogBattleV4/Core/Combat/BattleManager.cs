using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Actions;

namespace FrogBattleV4.Core.Combat;

public class BattleManager
{
    private readonly Dictionary<IBattleMember, IScheduledAction> _turns;
    private readonly Random _rng = new();
    public List<Team> AllTeams { get; }

    public BattleManager([NotNull] params Team[] teams)
    {
        AllTeams = teams.ToList();

        // Add every periodic turn to the turn dictionary
        _turns = AllTeams
            .SelectMany(team => team.Members
                // WARNING: This only handles characters. TODO: fix lmao
                .Select(IScheduledAction (bm) => new CharacterTurn
                {
                    Actor = bm,
                    SelectionProvider = team.Provider,
                }))
            .ToDictionary(action => action.Actor);

        // Schedule every turn at the start of battle
        Scheduler.ScheduleRange(_turns.Values);
    }

    public event EventHandler<BattleContext> TurnStart;
    public event EventHandler<BattleContext> TurnEnd;

    public TimelineScheduler Scheduler { get; } = new();

    public async Task RunAsync()
    {
        var turnNumber = 0L;
        while (Scheduler.MoveNext())
        {
            var member = Scheduler.Current.Actor;
            var allyTeam = member.GetAlliedTeam(AllTeams);
            var ctx = new BattleContext
            {
                Manager = this,
                ActiveMember = member,
                Allies = allyTeam?.Members,
                Enemies = AllTeams.SelectMany(team => team.Members).Except(allyTeam?.Members ?? []),
                TurnNumber = turnNumber++,
                Rng = _rng
            };
            TurnStart?.Invoke(this, ctx);
            member.Effects.TickStart();
            member.Pools.TickStart();
            var preview = await Scheduler.Current.PlayTurn(ctx);
            member.Effects.TickEnd();
            member.Pools.TickEnd();
            Scheduler.Schedule(_turns[member]);
            TurnEnd?.Invoke(this, ctx);
        }
        // TODO: Win logic lmao
    }

    private bool ExecuteAbility(AbilityExecContext ctx)
    {
        var preview = ctx.PreviewAbility();
        if (!preview.CanUse) return false;

        foreach (var command in preview.Commands)
        {
            ExecuteCommand(command, new ModifierContext
            {
                Actor = ctx.User,
                Other = ctx.MainTarget,
                Ability = ctx.Definition,
                Rng = ctx.Rng,
            });
        }

        return true;
    }

    private void ExecuteCommand(IBattleCommand cmd, ModifierContext ctx)
    {
        switch (cmd)
        {
            case Calculation.Damage.DamageCommand dc:
                dc.ExecuteDamage(ctx);
                break;
            case Calculation.Pools.MutationCommand mc:
                mc.Target.Pools.Mutate(mc.PreviewMutation(ctx));
                break;
            case Effects.ApplyEffectCommand aec:
                aec.Target.Effects.Apply(aec);
                break;
            case ActionAdvanceCommand aac:
                Scheduler.AdvancePercent(_turns[aac.Target], aac.AdvancePercent);
                break;
            case QueueActionCommand qac:
                Scheduler.Schedule(qac.Action);
                break;
            default:
                throw new NotSupportedException();
        }
    }
}