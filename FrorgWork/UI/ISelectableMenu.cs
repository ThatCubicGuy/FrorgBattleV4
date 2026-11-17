using FrorgWork.Input;
using Microsoft.Xna.Framework.Graphics;

namespace FrorgWork.UI;

public interface ISelectableMenu
{
    public void HandleInput(InputAction inputAction);
    public void Draw(SpriteBatch spriteBatch);
}