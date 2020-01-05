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
    private Transform cellBlockParent;
    [SerializeField]
    private GameObject cellBlockPrefab;

    int toolbeltSize = 7;
    Block[] toolbeltBlocks;
    Material[] mats;
    AudioClip[] audioClips;
    Block currentlySelected;

    private void Awake()
    {
        mats = Resources.LoadAll<Material>("Materials/Blocks");
        audioClips = Resources.LoadAll<AudioClip>("Audio/Etc");

        initializeToolbelt();
        drawToolbelt();
    }

    private void initializeToolbelt()
    {
        toolbeltBlocks = new Block[toolbeltSize];

        for (int i = 0; i < toolbeltSize; i++)
        {
            toolbeltBlocks[i] = new Block(-1, -1, -1, -1, mats[i].name, audioClips[i].name);
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
            spawnedButton.GetComponentInChildren<Text>().text = currentBlock.getAudio();
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

    public void drawSong(Song song)
    {
        Debug.Log("Drawing the song from memory...");

        GameObject spawnedCellBlock = null;
        CellBlock actualCellBlock = null;
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
                        //Debug.Log($"found a block with pos x: {block.x} y: {block.y} z: {block.z}");
                        Vector3 cellBlockPos = new Vector3(block.x, block.y, block.z);
                        instantiateCellBlock(actualCellBlock, cellBlockPos, spawnedCellBlock, block.getMaterial(), i, j);
                    }
                }
            }
        }
    }

    public Block placeCellBlock(Vector3 cellBlockPosition, int row, int rowIndex)
    {
        CellBlock actualCellBlock = null;
        GameObject spawnedCellBlock = null;
        string materialName = currentlySelected.getMaterial();
        string audioClipName = currentlySelected.getAudio();

        Debug.Log($"Placing cell block of type {materialName} with audio file {audioClipName}");
        cellBlockPosition.y += cellBlockPrefab.transform.localScale.y / 2 + 0.1f;
        instantiateCellBlock(actualCellBlock, cellBlockPosition, spawnedCellBlock, materialName, row, rowIndex);

        Block saveBlock = new Block(cellBlockPosition.x, cellBlockPosition.y, cellBlockPosition.z, rowIndex, materialName, audioClipName);

        return saveBlock;
    }

    private void instantiateCellBlock(CellBlock cellblock, Vector3 pos, GameObject spawnedCellBlock, string materialName, int row, int rowIndex)
    {
        spawnedCellBlock = Instantiate(cellBlockPrefab, pos, cellBlockPrefab.transform.rotation);
        spawnedCellBlock.GetComponent<Renderer>().material = Resources.Load($"Materials/Blocks/{materialName}", typeof(Material)) as Material;
        spawnedCellBlock.transform.SetParent(cellBlockParent, true);

        cellblock = spawnedCellBlock.GetComponent<CellBlock>();
        cellblock.row = row;
        cellblock.rowIndex = rowIndex;
    }
}
