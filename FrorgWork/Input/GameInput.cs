using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FrorgWork.Input;

/// <summary>
/// Handles basic keyboard input (on/off actions) and supports mapping multiple keys to the same action.
/// </summary>
public static class GameInput
{
    private static KeyboardState _previousKeyboardState;
    private static KeyboardState _currentKeyboardState;
    private static InputState _inputState;

    public static readonly Keymap DefaultKeymap = Keymap.GetBuilder()
        .Map(InputAction.Confirm, Keys.Z, Keys.Enter, Keys.Space)
        .Map(InputAction.Cancel, Keys.X, Keys.LeftShift)
        .Map(InputAction.Up, Keys.W, Keys.Up)
        .Map(InputAction.Down, Keys.S, Keys.Down)
        .Map(InputAction.Left, Keys.A, Keys.Left)
        .Map(InputAction.Right, Keys.D, Keys.Right)
        .Map(InputAction.Pause, Keys.Escape)
        .Build();

    public static IKeymap LoadedKeymap { get; set; } = DefaultKeymap;

    public static void Update(GameTime _)
    {
        _previousKeyboardState = _currentKeyboardState;
        _currentKeyboardState = Keyboard.GetState();

        _inputState = LoadedKeymap.Translate(_previousKeyboardState, _currentKeyboardState);
    }

    public static InputState GetState() => _inputState;

    public static InputStatus GetStatus(KeyState previous, KeyState current)
    {
        return GetStatus(previous == KeyState.Down, current == KeyState.Down);
    }

    public static InputStatus GetStatus(bool previous, bool current)
    {
        return (previous, current) switch
        {
            (false, false) => InputStatus.Idle,
            (false, true) => InputStatus.Pressed,
            (true, true) => InputStatus.Held,
            (true, false) => InputStatus.Released,
        };
    }
}