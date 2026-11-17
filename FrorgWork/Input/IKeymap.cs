using Microsoft.Xna.Framework.Input;

namespace FrorgWork.Input;

public interface IKeymap
{
    InputState Translate(KeyboardState previous, KeyboardState current);
}