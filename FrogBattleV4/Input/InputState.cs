using System;

namespace FrogBattleV4.Input;

public readonly struct InputState
{
    private readonly InputActions _previous = InputActions.None;
    private readonly InputActions _current = InputActions.None;

    private InputState(InputActions previous, InputActions current)
    {
        _previous = previous;
        _current = current;
    }

    public InputStatus this[InputAction action] =>
        (_previous.HasFlag(AsBit(action)), _current.HasFlag(AsBit(action))) switch
        {
            (false, false) => InputStatus.Idle,
            (false, true) => InputStatus.Pressed,
            (true, true) => InputStatus.Held,
            (true, false) => InputStatus.Released,
        };

    public InputState With(InputAction action, InputStatus status)
    {
        var (previous, current) = status switch
        {
            InputStatus.Idle => (_previous & ~AsBit(action), _current & ~AsBit(action)),
            InputStatus.Pressed => (_previous & ~AsBit(action), _current | AsBit(action)),
            InputStatus.Held => (_previous | AsBit(action), _current | AsBit(action)),
            InputStatus.Released => (_previous | AsBit(action), _current & ~AsBit(action)),
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null),
        };
        return new InputState(previous, current);
    }

    public InputState WithRestricted(InputAction action, InputStatus status)
    {
        return this[action] switch
        {
            // Idle and Held can be overridden
            InputStatus.Idle or InputStatus.Held => With(action, status),
            // Pressed and Released cannot be overridden
            InputStatus.Pressed or InputStatus.Released => this,
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null),
        };
    }

    private static InputActions AsBit(InputAction action) => action switch
    {
        InputAction.Confirm => InputActions.Confirm,
        InputAction.Cancel => InputActions.Cancel,
        InputAction.Up => InputActions.Up,
        InputAction.Down => InputActions.Down,
        InputAction.Left => InputActions.Left,
        InputAction.Right => InputActions.Right,
        InputAction.Pause => InputActions.Pause,
        _ => throw new ArgumentOutOfRangeException(nameof(action), action, "Unknown InputAction: " + action),
    };

    [Flags]
    private enum InputActions
    {
        None = 0,
        Confirm = 1 << 0,
        Cancel = 1 << 1,
        Up = 1 << 2,
        Down = 1 << 3,
        Left = 1 << 4,
        Right = 1 << 5,
        Pause = 1 << 6,
    }
}