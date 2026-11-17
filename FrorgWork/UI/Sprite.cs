using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrorgWork.UI;

/// <summary>
/// Represents a texture in a set location on screen.
/// </summary>
/// <param name="bounds">On-screen bounds of this sprite.</param>
/// <param name="texture">Texture that will be displayed for this sprite.</param>
[method: SetsRequiredMembers]
public readonly struct Sprite(Rectangle bounds, Texture2D texture)
{
    [SetsRequiredMembers]
    public Sprite(Point position, Texture2D texture)
        : this(new Rectangle(position.X, position.Y, texture.Width, texture.Height), texture) { }

    /// <summary>
    /// The on-screen bounds of this sprite.
    /// </summary>
    public required Rectangle Bounds { get; init; } = bounds;
    /// <summary>
    /// The displayed texture of this sprite.
    /// </summary>
    public required Texture2D Texture { get; init; } = texture;
}