using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Actions;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core.Combat;

public class BattleManager
{
    private readonly Dictionary<GameEntity, IScheduledAction> _turns;
    private readonly Random _rng = new();
    public List<Team> AllTeams { get; }

    public BattleManager(params Team[] teams)
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
}