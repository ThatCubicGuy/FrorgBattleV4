using Microsoft.Xna.Framework.Input;

namespace FrogBattleV4.Input;

public interface IKeymap
{
    InputState Translate(KeyboardState previous, KeyboardState current);
}