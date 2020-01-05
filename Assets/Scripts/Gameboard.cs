using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameboard : MonoBehaviour
{
    [SerializeField]
    private int numRows;
    [SerializeField]
    private int rowSize;
    [SerializeField]
    private float rowSpacing;
    [SerializeField]
    private GameObject cellPrefab;
    [SerializeField]
    private GameObject musicWallPrefab;

    float baseZ = -3f;
    float baseX = -30f;
    float baseY = -9f;

    private void Awake()
    {
        initializeCells();
        initializeMusicWall();
    }

    private void initializeCells()
    {
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
                spawnedCell.transform.SetParent(transform, true);
                actualCell = spawnedCell.GetComponent<Cell>();
                actualCell.row = row;
                actualCell.rowIndex = rowIndex;
            }
            spawnPos.z = baseZ;
            rowIndex = 0;
        }
    }

    private void initializeMusicWall()
    {
        GameObject spawnedMusicWall;
        float musicWallHeight = musicWallPrefab.transform.localScale.y;
        Vector3 musicWallPos = new Vector3(baseX - 2f, baseY + musicWallHeight / 2, rowSize + 1);

        spawnedMusicWall = Instantiate(musicWallPrefab, musicWallPos, musicWallPrefab.transform.rotation);
        spawnedMusicWall.transform.localScale += new Vector3(0, 0, rowSize * 3f);
        spawnedMusicWall.transform.SetParent(transform, true);
    }
}
