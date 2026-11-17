using System;
using System.Collections.Generic;
using FrorgWork.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrorgWork.UI;

// TODO (Nova): maybe make MenuBase a bit more useful externally?
/// <summary>
/// A class that provides implementations for common menu functions.
/// </summary>
public abstract class MenuBase(Point location, Texture2D cellTexture, Texture2D cursorTexture) : ISelectableMenu
{
    /// <summary>Gets a value indicating whether this menu currently accepts input.</summary>
    public bool Focused { get; set; }

    /// <summary>Top left corner of the menu, in pixels.</summary>
    public Point Location { get; } = location;

    /// <summary>Texture of the menu cells.</summary>
    public Texture2D CellTexture { get; } = cellTexture;

    /// <summary>Texture of the selected menu cell.</summary>
    public Texture2D CursorTexture { get; } = cursorTexture;

    public void HandleInput(InputAction inputAction)
    {
        if (!Focused) return;
        switch (inputAction)
        {
            case InputAction.Confirm:
                OnSelected();
                return;
            case InputAction.Cancel:
            case InputAction.Pause:
                Focused = false;
                return;
            case InputAction.Up:
            case InputAction.Down:
            case InputAction.Left:
            case InputAction.Right:
                Move(inputAction);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(inputAction), inputAction, null);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var sprite in GetAllSprites())
        {
            spriteBatch.Draw(sprite);
        }
    }

    protected abstract IEnumerable<Sprite> GetAllSprites();

    /// <summary>
    /// Moves the menu cursor to another cell given a directional InputAction.
    /// When this method is invoked, the <paramref name="direction"/> parameter
    /// is guaranteed to be one of Up, Down, Left, or Right (all of enum InputAction)
    /// </summary>
    /// <param name="direction">Direction in which to move the menu.</param>
    protected abstract void Move(InputAction direction);

    protected abstract void OnSelected();
}