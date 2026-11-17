using System;
using System.Collections.Generic;
using System.Linq;
using FrorgWork.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrorgWork.UI;

/// <summary>
/// A menu with a set amount of columns and a different amount of rows for each column.
/// </summary>
public class JaggedMenu<TItems> : MenuBase
{
    private readonly TItems[][] _columns;

    public event EventHandler<TItems>? Selected;

    /// <summary>
    /// A menu with a set amount of columns and a different amount of rows for each column.
    /// </summary>
    protected JaggedMenu(Point location, int columns, int columnOffset, int rowOffset, Point cellSize,
        Texture2D cellTexture, Texture2D cursorTexture) : base(location, cellTexture, cursorTexture)
    {
        _columns = new TItems[columns][];
        Columns = columns;
        RowOffset = rowOffset;
        CellSize = cellSize;
        ColumnOffset = columnOffset;
    }

    public JaggedMenuOptions Options { get; protected init; }

    /// <summary>
    /// Initializes the specified column with an array of the given length.
    /// </summary>
    /// <param name="column">Column to initialize.</param>
    /// <param name="length">Amount of rows that this column will have.</param>
    protected void Initialize(int column, int length)
    {
        _columns[column] = new TItems[length];
    }

    protected override IEnumerable<Sprite> GetAllSprites()
    {
        for (var i = 0; i < _columns.Length; i++)
        {
            var column = _columns[i];
            for (var j = 0; j < column.Length; j++)
            {
                yield return new Sprite(
                    new Rectangle(
                        Location + new Point(
                            i * (CellSize.X + ColumnOffset),
                            j * (CellSize.Y + RowOffset)),
                        CellSize), SelectedColumn == i && SelectedRow == j ? CursorTexture : CellTexture);
            }
        }
    }

    protected override void Move(InputAction direction)
    {
        switch (direction)
        {
            case InputAction.Up:
                SelectedRow -= 1;
                break;
            case InputAction.Down:
                SelectedRow += 1;
                break;
            case InputAction.Left:
                SelectedColumn -= 1;
                break;
            case InputAction.Right:
                SelectedColumn += 1;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    protected override void OnSelected()
    {
        Selected?.Invoke(this, _columns[SelectedColumn][SelectedRow]);
    }

    private int SelectedColumn
    {
        get;
        set => field = Math.Clamp(value, 0, Columns - 1);
    }

    private int SelectedRow
    {
        get => Math.Clamp(field, 0, Rows - 1);
        set => field = Math.Clamp(value, 0, Rows - 1);
    }

    /// <summary>
    /// Gets the amount of columns in this jagged menu.
    /// </summary>
    public int Columns { get; }

    /// <summary>
    /// Gets the amount of rows on the current column.
    /// </summary>
    public int Rows => _columns[SelectedColumn].Length;
    public int RowOffset { get; }
    public int ColumnOffset { get; }

    /// <summary>Size of each individual cell, in pixels.</summary>
    public Point CellSize { get; }

    /// <summary>
    /// Gets the sprite at the specified index in the menu.
    /// </summary>
    /// <param name="col">Zero-based index of the column.</param>
    /// <param name="row">Zero-based index of the row on the current column.</param>
    public TItems this[int col, int row] => _columns[col][row];
}

public readonly record struct JaggedMenuOptions(
    JaggedMenuAlignment Alignment);

public enum JaggedMenuAlignment : byte
{
    Top,
    Center,
    Bottom,
}