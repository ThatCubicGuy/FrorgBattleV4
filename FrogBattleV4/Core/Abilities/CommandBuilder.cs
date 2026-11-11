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
    /// Flushes every currently loaded command and accepts the visitor for each.
    /// Any command added during flushing is not processed nor removed.
    /// </summary>
    /// <param name="visitor">Command visitor to accept.</param>
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

    /// <summary>
    /// Accepts the visitor for each command loaded, then clears the queue.
    /// Any commands added during flushing are ignored and removed.
    /// </summary>
    /// <param name="visitor">Command visitor to accept.</param>
    public void FlushClean(ICommandVisitor visitor)
    {
        Flush(visitor);
        _commandBuffer.Clear();
    }
}