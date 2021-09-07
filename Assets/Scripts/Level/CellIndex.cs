using UnityEngine;

/// <summary>
/// Data structure to define cell index.
/// </summary>
[System.Serializable]
public struct CellIndex
{
    [SerializeField] private int rowIndex;
    [SerializeField] private int columnIndex;

    public CellIndex(int rowIndex, int columnIndex)
    {
        this.rowIndex = rowIndex;
        this.columnIndex = columnIndex;
    }

    public int RowIndex
    {
        get => rowIndex;
        set => rowIndex = value;
    }

    public int ColumnIndex
    {
        get => columnIndex;
        set => columnIndex = value;
    }
}