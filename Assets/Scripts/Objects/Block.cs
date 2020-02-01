using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Block
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
    public bool isLoopBlock;

    protected int index;
    protected string materialPath;
    protected string audioPath;

    public Block(float x, float y, float z, int index, string materialPath, string audioPath)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.index = index;
        this.materialPath = materialPath;
        this.audioPath = audioPath;
        // should eventually separate special blocks into sub-class of Block
        isLoopBlock = false;
    }

    public int getIndex()
    {
        return index;
    }

    public string getMaterial()
    {
        return materialPath;
    }

    public string getAudio()
    {
        return audioPath;
    }

    public void setMaterial(string materialPath)
    {
        this.materialPath = materialPath;
    }
}