using System.Collections.Generic;
using FrogBattleV4.Core.Combat.Commands;

namespace FrogBattleV4.Core.Abilities;

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