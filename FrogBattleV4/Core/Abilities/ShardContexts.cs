using System.Collections.Generic;
using FrogBattleV4.Core.Abilities.Components;
using FrogBattleV4.Core.Combat.Commands;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.Abilities;

/// <summary>
/// A context describing the current state of resolution.
/// </summary>
/// <param name="User">Entity using this link.</param>
/// <param name="Selections">Currently selected targets.</param>
/// <param name="Modifiers">Currently active modifiers.</param>
/// <param name="CurrentShardIndex">Index of the currently resolving shard.</param>
public sealed record LinkResolutionState(
    EntityUid User,
    IEnumerable<ShardTargetingContext> Selections,
    ModifierCollection Modifiers,
    int CurrentShardIndex);

/// <summary>
/// Small scope object for a single shard command's resolution.
/// </summary>
/// <param name="User">User of the shard.</param>
/// <param name="Targeting">Target of the shard.</param>
/// <param name="Modifier">Modifiers owned by the user at time of resolution.</param>
public record ShardResolutionScope(
    EntityUid User,
    ShardTargetingContext Targeting,
    // TODO (Nova): actually use bonus modifiers lmao
    IModifierProvider Modifier);

public sealed class CommandBuilder
{
    private readonly List<BattleCommand> _commands = [];
    
    public void Emit(BattleCommand command)
    {
        _commands.Add(command);
    }

    public void Build(ICommandVisitor visitor)
    {
        foreach (var command in _commands)
        {
            command.Accept(visitor);
        }
    }
}