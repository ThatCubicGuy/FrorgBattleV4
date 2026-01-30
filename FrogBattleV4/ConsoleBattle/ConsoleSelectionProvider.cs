using System;
using System.Threading.Tasks;
using FrogBattleV4.Core.BattleSystem.Decisions;

namespace FrogBattleV4.ConsoleBattle;

public class ConsoleSelectionProvider : IDecisionProvider
{
    public async Task<DecisionResult<TResult>> GetSelectionAsync<TResult>(IDecisionRequest<TResult> request)
    {
        return (DecisionResult<TResult>)(request switch
        {
            AbilityDecisionRequest ab => await GetAbilitySelectionAsync(ab),
            TargetDecisionRequest tg => await GetTargetSelectionAsync(tg),
            _ => throw new NotSupportedException($"Decision type {typeof(TResult).Name} not supported")
        });
    }

    private static async Task<IDecisionResult> GetAbilitySelectionAsync(AbilityDecisionRequest request)
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

                return selection < 0 || selection >= request.ValidOptions.Count
                    ? throw new IndexOutOfRangeException(nameof(selection))
                    : request.Select(selection);
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

    private static async Task<IDecisionResult> GetTargetSelectionAsync(TargetDecisionRequest request)
    {
        while (true)
        {
            try
            {
                await Console.Out.WriteLineAsync($"Select target for {request.Requestor.Name}: ");
                if (int.TryParse(await Task.Run(Console.ReadLine), out var selection) &&
                    selection >= 0 && selection < request.ValidOptions.Count)
                {
                    return request.Select(selection);
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