using UnityEngine;

/// <summary>
/// Utility class for map related conversions.
/// </summary>
public class MapUtils
{
    // Position of first cell at (0,0)
    private static readonly Vector2 FirstCellPosition = new Vector2(-5.5f, 4.5f);

    // Cell dimensions
    private static readonly int CellWidth = 1;
    private static readonly int CellHeight = 1;

    /// <summary>
    /// Snaps given position to the center of the nearest cell.
    /// </summary>
    /// <param name="position"></param>
    /// <returns>The centered position</returns>
    public static Vector3 SnapToCenter(Vector3 position) => GetWorldPosition(GetCellIndices(position));

    /// <summary>
    /// Gets the position of the given cell index in the world.
    /// </summary>
    /// <param name="cellIndex"></param>
    /// <returns>The position of the cell</returns>
    public static Vector2 GetWorldPosition(CellIndex cellIndex) =>
        FirstCellPosition + Vector2.down * CellHeight * cellIndex.RowIndex + Vector2.right * CellWidth * cellIndex.ColumnIndex;

    /// <summary>
    /// Gets the cell indices of the given position.
    /// </summary>
    /// <param name="position"></param>
    /// <returns>The cell index of the position</returns>
    public static CellIndex GetCellIndices(Vector2 position)
    {
        Vector2 positionWithoutOffset = position - FirstCellPosition;

        return new CellIndex(Mathf.RoundToInt(Mathf.Abs(positionWithoutOffset.y / CellHeight)),
                           Mathf.RoundToInt(positionWithoutOffset.x / CellWidth));
    }
}