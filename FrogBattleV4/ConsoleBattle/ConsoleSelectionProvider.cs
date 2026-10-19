using System;
using System.Threading;
using System.Threading.Tasks;
using FrogBattleV4.Core.Selections;

namespace FrogBattleV4.ConsoleBattle;

public class ConsoleSelectionProvider : ISelectionProvider
{
    public async Task<PlayerTurnAction> GetSelectionAsync(PlayerActionRequest request, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}