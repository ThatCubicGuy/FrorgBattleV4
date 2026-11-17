using FrogBattleV4.Core.Abilities;
using FrorgWork.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrogBattleV4.UI;

public class JaggedShardMenu : JaggedMenu<ShardId>
{
    public JaggedShardMenu(Point location, Texture2D cellTexture, Texture2D cursorTexture) : base(
        location, 3, 10, 5, cellTexture.Bounds.Size * 2, cellTexture, cursorTexture)
    {
        Focused = true;
        Initialize(0, 2);
        Initialize(1, 5);
        Initialize(2, 3);
    }
}