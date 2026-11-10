using System.Collections.Generic;
using FrogBattleV4.Core.Combat.Commands;

namespace FrogBattleV4.Core.Abilities;

public sealed class CommandBuilder
{
    private readonly List<BattleCommand> _commandBuffer = new(16);

    public void Emit(BattleCommand command)
    {
        _commandBuffer.Add(command);
    }

    /// <summary>
    /// Accepts the visitor for each command loaded, then clears the entire queue.
    /// </summary>
    /// <param name="visitor"></param>
    public void Flush(ICommandVisitor visitor)
    {
        // Prevent InvalidOperationExceptions during runtime if commands get added while flushing
        var commands = _commandBuffer.ToArray();
        _commandBuffer.Clear();

        foreach (var command in commands)
        {
            command.Accept(visitor);
        }
    }
}