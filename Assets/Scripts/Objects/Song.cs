using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Song
{
    List<Row> rows;

    public List<Row> getRows()
    {
        return rows;
    }

    public Song()
    {
        rows = new List<Row>();
    }

    public Row GetRowByIndex(int index)
    {
        if (rows.Count > index)
        {
            return rows[index];
        }
        else
        {
            return null;
        }
    }

    public void AddBlockToRow(Block block, int row, int rowIndex)
    {
        if (rows.Count <= row)
        {
            while (rows.Count <= row)
            {
                rows.Add(new Row());
            }
        }
        Debug.Log($"Inserting in row {row}...");
        rows[row].AddNoteByIndex(rowIndex, block);
    }
}