using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Selections;

namespace FrogBattleV4.ConsoleBattle;

public class ConsoleSelectionProvider : ISelectionProvider
{
    public async Task<PlayerTurnAction> GetSelectionAsync(PlayerActionRequest request, CancellationToken token)
    {
        await Console.Out.WriteLineAsync("Select shards:");
        var shards = request.Environment
            .GetShards(request.User)
            .Select(Core.Registries.ShardRegistry.Global.Get)
            .ToArray();
        foreach (var (i, shard) in shards.Index())
        {
            await Console.Out.WriteLineAsync($"{i + 1}: {shard.Name}");
        }

        var link = new List<IShard>();

        while (true)
        {
            await Console.Out.WriteAsync($"Shard: ");
            var nextLine = await Console.In.ReadLineAsync(token);

            if (uint.TryParse(nextLine, out var selection) && selection <= shards.Length)
            {
                if (selection == 0) break;

                link.Add(shards[selection - 1]);
            }
            else
            {
                await Console.Out.WriteLineAsync($"Invalid selection: \"{nextLine}\"");
            }
        }

        while (true)
        {
            await Console.Out.WriteAsync($"Target: ");
            var nextLine = await Console.In.ReadLineAsync(token);

            if (int.TryParse(nextLine, out var selection) && selection <= shards.Length)
            {
                return new PlayerTurnAction(link.ToImmutableList(),
                    request.Environment.GetEnemies(request.User).ElementAt(selection - 1));
            }

            await Console.Out.WriteLineAsync($"Invalid target selection \"{nextLine}\"");
        }
    }
}