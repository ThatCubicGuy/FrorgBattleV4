// using var game = new FrogBattleV4.FrogBattle();
// game.Run();

using System.Threading;
using FrogBattleV4.ConsoleBattle;
using FrogBattleV4.Core.Combat;

var provider = new ConsoleSelectionProvider(CancellationToken.None);
var game = new BattleManager(provider, new Team(), new Team());
var runningTask = game.RunAsync();
while (true)
{
    // Avoid public execution with this one simple trick:
    // TODO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
}