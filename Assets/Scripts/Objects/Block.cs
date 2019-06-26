using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Block
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }

    protected int index;
    protected string materialPath;

    public Block(float x, float y, float z, int index, string materialPath)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.index = index;
        this.materialPath = materialPath;
    }

    public int getIndex()
    {
        return index;
    }

    public string getMaterial()
    {
        return materialPath;
    }

    public void setMaterial(string materialPath)
    {
        this.materialPath = materialPath;
    }
}