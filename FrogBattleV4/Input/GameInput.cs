using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.Xna.Framework.Input;

namespace FrogBattleV4.Input;

public static class GameInput
{
    public static readonly ImmutableDictionary<Keys, InputCommand> KeyMap = new Dictionary<Keys, InputCommand>
    {
        [Keys.Z] = InputCommand.Confirm,
        [Keys.X] = InputCommand.Cancel,
        [Keys.Up] = InputCommand.Up,
        [Keys.Down] = InputCommand.Down,
        [Keys.Left] = InputCommand.Left,
        [Keys.Right] = InputCommand.Right,
        [Keys.Enter] = InputCommand.Start,
        [Keys.Escape] = InputCommand.Escape,
    }.ToImmutableDictionary();
}

public enum InputCommand
{
    Confirm,
    Cancel,
    Up,
    Down,
    Left,
    Right,
    Start,
    Escape,
}