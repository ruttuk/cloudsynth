using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlockManager : MonoBehaviour
{
    [SerializeField]
    private float tbYOffset;
    [SerializeField]
    private GameObject toolbeltButtonPrefab;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private GameObject cellPrefab;
    [SerializeField]
    private Transform cellParent;
    [SerializeField]
    private GameObject cellBlockPrefab;
    [SerializeField]
    private int numRows;
    [SerializeField]
    private int rowSize;
    [SerializeField]
    private float rowSpacing;

    int toolbeltSize = 7;
    Block[] toolbeltBlocks;
    Material[] mats;
    Block currentlySelected;

    private void Awake()
    {
        mats = Resources.LoadAll<Material>("Materials/Blocks");

        initializeCells();
        initializeToolbelt();
        drawToolbelt();
    }

    private void initializeToolbelt()
    {
        toolbeltBlocks = new Block[toolbeltSize];

        for (int i = 0; i < toolbeltSize; i++)
        {
            toolbeltBlocks[i] = new Block(-1, -1, -1, -1, mats[i].name);
        }

        currentlySelected = toolbeltBlocks[0];
    }

    private void drawToolbelt()
    {
        Vector3 spawnPos = spawnPoint.transform.position;
        spawnPos.y += tbYOffset;
        GameObject spawnedButton;

        for (int i = 0; i < toolbeltSize; i++)
        {
            Block currentBlock = toolbeltBlocks[i];
            spawnedButton = Instantiate(toolbeltButtonPrefab, spawnPos, toolbeltButtonPrefab.transform.rotation);
            spawnedButton.SetActive(true);
            spawnedButton.transform.SetParent(spawnPoint, false);
            spawnedButton.GetComponentInChildren<Text>().text = currentBlock.getMaterial();
            spawnedButton.GetComponent<Image>().material = mats[i];
            spawnedButton.GetComponentInChildren<Button>().onClick.AddListener(delegate { setCurrentBlock(currentBlock); });
            spawnPos.x += 50f;
        }
    }

    private void setCurrentBlock(Block block)
    {
        currentlySelected = block;
        Debug.Log($"Current block is now {currentlySelected.getMaterial()}!");
    }

    private void initializeCells()
    {
        float baseZ = -3f;
        float baseX = -30f;
        float baseY = -9f;
        Vector3 spawnPos = new Vector3(baseX, baseY, baseZ);
        GameObject spawnedCell;
        Cell actualCell;
        int row = 0;
        int rowIndex = 0;

        for (float i = baseX; i < numRows * rowSpacing + baseX; i += rowSpacing, row++)
        {
            spawnPos.x = i;
            for (float j = baseZ; j < rowSize * rowSpacing + baseZ; j += rowSpacing, rowIndex++)
            {
                spawnPos.z = j;
                spawnedCell = Instantiate(cellPrefab, spawnPos, cellPrefab.transform.rotation);
                spawnedCell.transform.SetParent(cellParent, true);
                actualCell = spawnedCell.GetComponent<Cell>();
                actualCell.row = row;
                actualCell.rowIndex = rowIndex;
            }
            spawnPos.z = baseZ;
            rowIndex = 0;
        }
    }

    public void drawSong(Song song)
    {
        Debug.Log("Drawing the song from memory...");
        GameObject spawnedCellBlock;
        Row row;
        Block block;

        int numRows = song.getRows().Count;
        int rowSize = 10;

        for(int i = 0; i < numRows; i++)
        {
            row = song.GetRowByIndex(i);
            if (row != null)
            {
                for (int j = 0; j < rowSize; j++)
                {
                    block = row.GetBlockByIndex(j);
                    if (block != null)
                    {
                        Debug.Log($"found a block with pos x: {block.x} y: {block.y} z: {block.z}");
                        Vector3 cellBlockPos = new Vector3(block.x, block.y, block.z);
                        spawnedCellBlock = Instantiate(cellBlockPrefab, cellBlockPos, cellBlockPrefab.transform.rotation);
                        spawnedCellBlock.GetComponent<Renderer>().material = Resources.Load($"Materials/Blocks/{block.getMaterial()}", typeof(Material)) as Material;
                    }
                }
            }
        }
    }

    public Block placeCellBlock(Vector3 cellBlockPosition, int rowIndex)
    {
        Debug.Log($"Placing cell block of type {currentlySelected.getMaterial()}");
        cellBlockPosition.y += cellBlockPrefab.transform.localScale.y / 2 + 0.1f;
        GameObject spawnedCellBlock = Instantiate(cellBlockPrefab, cellBlockPosition, cellBlockPrefab.transform.rotation);
        Material saveMat = Resources.Load($"Materials/Blocks/{currentlySelected.getMaterial()}", typeof(Material)) as Material;
        spawnedCellBlock.GetComponent<Renderer>().material = saveMat;
        Block saveBlock = new Block(cellBlockPosition.x, cellBlockPosition.y, cellBlockPosition.z, rowIndex, saveMat.name);

        return saveBlock;
    }
}
