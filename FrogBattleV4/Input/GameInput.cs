using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FrogBattleV4.Input;

public static class GameInput
{
    private static KeyboardState _previousKeyboardState;
    private static KeyboardState _currentKeyboardState;
    private static InputState _inputState;

    public static readonly Keymap DefaultKeymap = Keymap.GetBuilder()
        .Map(InputAction.Confirm, Keys.Z, Keys.Enter)
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
}