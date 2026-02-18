using System;
using System.Linq;
using System.Threading.Tasks;
using FrogBattleV4.Core.BattleSystem.Selections;

namespace FrogBattleV4.ConsoleBattle;

public class ConsoleSelectionProvider : ISelectionProvider
{
    public async Task<ISelectionResult<TResult>> GetSelectionAsync<TResult>(ISelectionRequest<TResult> request)
    {
        return request.Select(request switch
        {
            AbilitySelectionRequest ab => await GetAbilitySelectionAsync(ab),
            TargetSelectionRequest tg => await GetTargetSelectionAsync(tg),
            _ => throw new NotSupportedException($"Decision type {typeof(TResult).Name} not supported")
        });
    }

    private static async Task<int> GetAbilitySelectionAsync(AbilitySelectionRequest request)
    {
        while (true)
        {
            try
            {
                var selection = 0;
                await Console.Out.WriteAsync($"Select ability for {request.Requestor.Name}: ");
                try
                {
                    selection = int.Parse(await Task.Run(Console.ReadLine) ?? string.Empty);
                }
                catch (FormatException ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                }

                return selection < 0 || selection >= request.ValidOptions.Count()
                    ? throw new IndexOutOfRangeException(nameof(selection))
                    : selection;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex switch
                {
                    IndexOutOfRangeException or OverflowException => "Invalid ability number!",
                    _ => ex.Message
                });
            }
        }
    }

    private static async Task<int> GetTargetSelectionAsync(TargetSelectionRequest request)
    {
        while (true)
        {
            try
            {
                await Console.Out.WriteLineAsync($"Select target for {request.Requestor.Name}: ");
                if (int.TryParse(await Task.Run(Console.ReadLine), out var selection) &&
                    selection >= 0 && selection < request.ValidOptions.Count())
                {
                    return selection;
                }

                await Console.Out.WriteLineAsync("Invalid target number!");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex switch
                {
                    IndexOutOfRangeException or OverflowException => "Invalid target number!",
                    _ => ex.Message
                });
            }
        }
    }
}