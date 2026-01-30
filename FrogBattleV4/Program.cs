// using var game = new FrogBattleV4.FrogBattle();
// game.Run();

using FrogBattleV4.ConsoleBattle;
using FrogBattleV4.Content.Characters;
using FrogBattleV4.Core.BattleSystem;

var c1 = new Ayumi();
var c2 = new Ayumi();
var provider = new ConsoleSelectionProvider();
var game = new BattleManager(provider, new Team(c1), new Team(c2));
var runningTask = game.RunAsync();
while (true)
{
    
}