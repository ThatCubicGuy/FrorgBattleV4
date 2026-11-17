using FrogBattleV4.Core.Abilities;
using FrorgWork.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrogBattleV4.UI;

public class ShardMenu : RectangularMenu<ShardId>
{
    public ShardMenu(int columns, int rows, Point location, Texture2D cellTexture, Texture2D cursorTexture) : base(
        location, columns, rows, cellTexture.Bounds.Size * 2, new Point(10, 30), cellTexture, cursorTexture)
    {
        Focused = true;
    }
}