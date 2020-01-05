using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    // return only the blocks that are currently holding data (i.e. not null)
    public Block[] getAllOccupiedBlocks()
    {
        int nonNull = blocks.Count(b => b != null);
        Block[] occupiedBlocks = new Block[nonNull];

        for(int i = 0, j = 0; i < rowSize; i++)
        {
            if(blocks[i] != null)
            {
                occupiedBlocks[j] = blocks[i];
                j++;
            }
        }

        return occupiedBlocks;      
    }

    public Row()
    {
        blocks = new Block[rowSize];
    }

    public Block GetBlockByIndex(int index)
    {
        return blocks[index];
    }

    public void DeleteBlockByIndex(int index)
    {
        Debug.Log($"Deleted block at index {index}");
        blocks[index] = null;
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
