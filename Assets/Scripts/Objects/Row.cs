using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Row
{
    int rowSize = 10;
    Block[] blocks;

    public int GetRowSize()
    {
        return rowSize;
    }

    public Block[] getBlocks()
    {
        return blocks;
    }

    public Row()
    {
        blocks = new Block[rowSize];
    }

    public Block GetBlockByIndex(int index)
    {
        return blocks[index];
    }

    public void AddNoteByIndex(int index, Block block)
    {
        if (index >= 0 || index < rowSize)
        {
            blocks[index] = block;
            Debug.Log($"Added block at index {index} with material {block.getMaterial()} and coordinates {block.x}, {block.y}, {block.z}!");
        }
        else
        {
            Debug.Log("Cannot add Block to row, index out of range!!!");
        }
    }
}
