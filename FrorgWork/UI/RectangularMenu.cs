using System;
using System.Collections.Generic;
using FrorgWork.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrorgWork.UI;

/// <summary>
/// A selectable menu containing cells of identical size.
/// </summary>
public class RectangularMenu<TItems> : MenuBase
{
    private readonly TItems[,] _items;

    public event EventHandler<TItems>? Selected;

    /// <summary>
    /// Initializes a new instance of a rectangular menu.
    /// </summary>
    /// <param name="columns">How many cells wide this menu is.</param>
    /// <param name="rows">How many cells tall this menu is.</param>
    /// <param name="location">Top left corner of the menu, in pixels.</param>
    /// <param name="cellSize">Size of each individual cell, in pixels.</param>
    /// <param name="cellOffset">Vertical and horizontal distance between cells, in pixels.</param>
    /// <param name="cellTexture">Texture to be rendered in every cell of the menu.</param>
    /// <param name="cursorTexture">Texture to be rendered for the selected cell of the menu.</param>
    protected RectangularMenu(Point location, int columns, int rows, Point cellSize, Point cellOffset,
        Texture2D cellTexture, Texture2D cursorTexture) : base(location, cellTexture, cursorTexture)
    {
        _items = new TItems[columns, rows];
        Columns = columns;
        Rows = rows;
        CellSize = cellSize;
        CellOffset = cellOffset;
    }

    /// <summary>How many cells wide this menu is.</summary>
    public int Columns { get; }

    /// <summary>How many cells tall this menu is.</summary>
    public int Rows { get; }

    /// <summary>Vertical and horizontal distance between cells, in pixels.</summary>
    public Point CellOffset { get; }

    public int SelectedRow
    {
        get;
        set => field = Math.Clamp(value, 0, Rows - 1);
    }

    public int SelectedColumn
    {
        get;
        set => field = Math.Clamp(value, 0, Columns - 1);
    }

    /// <summary>Size of each individual cell, in pixels.</summary>
    public Point CellSize { get; }

    /// <summary>
    /// Gets the sprite at the specified index in the table.
    /// </summary>
    /// <param name="col">Zero-based index of the column.</param>
    /// <param name="row">Zero-based index of the row.</param>
    public TItems this[int col, int row] => _items[col, row];

    protected override IEnumerable<Sprite> GetAllSprites()
    {
        for (var i = 0; i < Rows; i++)
        {
            for (var j = 0; j < Columns; j++)
            {
                yield return new Sprite(new Rectangle(
                    Location + new Point(
                        j * (CellSize.X + CellOffset.X),
                        i * (CellSize.Y + CellOffset.Y)),
                    CellSize), SelectedColumn == j && SelectedRow == i ? CursorTexture : CellTexture);
            }
        }
    }

    protected override void Move(InputAction direction)
    {
        (SelectedColumn, SelectedRow) = direction switch
        {
            InputAction.Up => (SelectedColumn, SelectedRow - 1),
            InputAction.Down => (SelectedColumn, SelectedRow + 1),
            InputAction.Left => (SelectedColumn - 1, SelectedRow),
            InputAction.Right => (SelectedColumn + 1, SelectedRow),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, ""),
        };
    }

    protected override void OnSelected()
    {
        Selected?.Invoke(this, _items[SelectedColumn, SelectedRow]);
    }
}