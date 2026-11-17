using System;

namespace FrorgWork.Input;

public readonly struct InputState
{
    private readonly InputActions _previous = InputActions.None;
    private readonly InputActions _current = InputActions.None;

    private InputState(InputActions previous, InputActions current)
    {
        _previous = previous;
        _current = current;
    }

    /// <summary>
    /// Gets the input status for a specified action.
    /// </summary>
    /// <param name="action">Action to check.</param>
    public InputStatus this[InputAction action] =>
        GameInput.GetStatus(_previous.HasFlag(AsBit(action)), _current.HasFlag(AsBit(action)));

    /// <summary>
    /// Checks whether input for the given action is in a pressed position on the current frame.
    /// </summary>
    /// <param name="action">Action to check input for.</param>
    /// <returns>True if the action was pressed or held during this input frame, false otherwise.</returns>
    public bool IsPressed(InputAction action)
    {
        return this[action] is InputStatus.Pressed or InputStatus.Held;
    }

    /// <summary>
    /// Checks whether input for the given action is in a released position on the current frame.
    /// </summary>
    /// <param name="action">Action to check input for.</param>
    /// <returns>True if the action was released or idle during this input frame, false otherwise.</returns>
    public bool IsReleased(InputAction action)
    {
        return this[action] is InputStatus.Released or InputStatus.Idle;
    }

    /// <summary>
    /// Returns a new InputState with the same values as the current one except for the given action.
    /// The value of the current InputState for that action is only overridden if it is either
    /// InputStatus.Idle or InputStatus.Held, giving priority to Pressed and Released.
    /// </summary>
    /// <param name="action">Action to try overriding.</param>
    /// <param name="status">InputStatus to try setting it to.</param>
    /// <returns>A new InputState.</returns>
    public InputState With(InputAction action, InputStatus status)
    {
        return this[action] switch
        {
            // Idle and Held can be overridden
            InputStatus.Idle or InputStatus.Held => WithStatus(action, status),
            // Pressed and Released cannot be overridden
            InputStatus.Pressed or InputStatus.Released => this,
            _ => throw new InvalidOperationException("Unreachable."),
        };
    }

    /// <summary>
    /// Returns a copy of this struct with the input status of a specified action modified.
    /// </summary>
    /// <param name="action">Action whose input status to modify.</param>
    /// <param name="status">Value to set the action's status to.</param>
    /// <returns>A new <see cref="InputState"/> value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="status"/> represents an invalid <see cref="InputStatus"/> value.
    /// </exception>
    private InputState WithStatus(InputAction action, InputStatus status)
    {
        var (previous, current) = status switch
        {
            // Unset previous and current
            InputStatus.Idle => (_previous & ~AsBit(action), _current & ~AsBit(action)),
            // Unset previous, set current
            InputStatus.Pressed => (_previous & ~AsBit(action), _current | AsBit(action)),
            // Set previous and current
            InputStatus.Held => (_previous | AsBit(action), _current | AsBit(action)),
            // Set previous, unset current
            InputStatus.Released => (_previous | AsBit(action), _current & ~AsBit(action)),
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null),
        };
        return new InputState(previous, current);
    }

    /// <summary>
    /// Converts a value of the <see cref="InputAction"/> enum into its corresponding
    /// <see cref="InputActions"/> bit field representation.
    /// </summary>
    /// <param name="action">Value to convert.</param>
    /// <returns>A value of type <see cref="InputActions"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="action"/> is out of the range of enum <see cref="InputAction"/>.
    /// </exception>
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