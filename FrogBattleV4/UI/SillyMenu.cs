using FrorgWork.Input;
using FrorgWork.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrogBattleV4.UI;

public class SillyMenu(Point location, Texture2D cellTexture)
    : ISelectableMenu
{
    public Sprite Cell { get; set; } = new(new Rectangle(location, cellTexture.Bounds.Size * 2), cellTexture);

    public void HandleInput(InputAction inputAction)
    {
        Cell = Cell with
        {
            Bounds = Cell.Bounds with
            {
                Location = Cell.Bounds.Location + inputAction switch
                {
                    InputAction.Up => new Point(0, -10),
                    InputAction.Down => new Point(0, 10),
                    InputAction.Left => new Point(-10, 0),
                    InputAction.Right => new Point(10, 0),
                    _ => default,
                },
            },
        };
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Cell);
    }
}