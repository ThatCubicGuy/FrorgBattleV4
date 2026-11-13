using Microsoft.Xna.Framework.Input;

namespace FrogBattleV4.Input;

public enum InputStatus
{
    Idle,
    Pressed,
    Released,
    Held,
}

public static class InputStatusExtensions
{
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

    public static InputStatus GetStatus(KeyState previous, KeyState current)
    {
        return GetStatus(previous == KeyState.Down, current == KeyState.Down);
    }

    public static bool IsPressed(this InputStatus inputStatus)
    {
        return inputStatus is InputStatus.Pressed or InputStatus.Held;
    }

    public static bool IsReleased(this InputStatus inputStatus)
    {
        return inputStatus is InputStatus.Released or InputStatus.Idle;
    }
}