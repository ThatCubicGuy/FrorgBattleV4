using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FrogBattleV4.Core.Combat.Actions;
using FrogBattleV4.Core.Combat.Selections;

namespace FrogBattleV4.Core.Combat;

public class BattleManager
{
    private readonly ISelectionProvider _playerInputInterface;
    private readonly Random _rng = new();
    public List<Team> AllTeams { get; init; }

    public BattleManager(ISelectionProvider provider, [NotNull] params Team[] teams)
    {
        AllTeams = teams.ToList();
        _playerInputInterface = provider;
        Scheduler.ScheduleRange(AllTeams
            .SelectMany(team => team.Members)
            .SelectMany(tt => tt.Turns));
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
                ActiveMember = member,
                Allies = allyTeam?.Members,
                Enemies = AllTeams.SelectMany(team => team.Members).Except(allyTeam?.Members ?? []),
                TurnNumber = ++turnNumber,
                Rng = _rng
            };
            TurnStart?.Invoke(this, ctx);
            await Scheduler.Current.PlayTurn(_playerInputInterface, ctx);
            TurnEnd?.Invoke(this, ctx);
            if (Scheduler.Current.InstantRequeue) Scheduler.Schedule(Scheduler.Current);
        }
        // TODO: Win logic lmao
    }
}