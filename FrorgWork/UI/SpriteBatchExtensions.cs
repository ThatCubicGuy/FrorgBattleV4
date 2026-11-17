using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrorgWork.UI;

public static class SpriteBatchExtensions
{
    public static void Draw(this SpriteBatch spriteBatch, Sprite sprite)
    {
        spriteBatch.Draw(sprite.Texture, sprite.Bounds, Color.White);
    }

    public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, Color color)
    {
        spriteBatch.Draw(sprite.Texture, sprite.Bounds, color);
    }
}