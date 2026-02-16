// using var game = new FrogBattleV4.FrogBattle();
// game.Run();

using FrogBattleV4.ConsoleBattle;
using FrogBattleV4.Core.BattleSystem;

var provider = new ConsoleSelectionProvider();
var game = new BattleManager(provider, new Team(), new Team());
var runningTask = game.RunAsync();
while (true)
{
    // Avoid public execution with this one simple truck:
    // TODO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
}