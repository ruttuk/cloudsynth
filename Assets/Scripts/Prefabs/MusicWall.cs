using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicWall : MonoBehaviour
{
    bool paused;
    float speed;
    Emcee MC;
    Vector3 originalPosition;
    AudioSource[] sources;
    private int currentRow;

    Block loopTail;
    Block loopHead;

    bool loopCompleted;

    void Awake()
    {
        paused = true;
        speed = 10f;
        currentRow = -1;
        sources = GetComponentsInChildren<AudioSource>();
        MC = FindObjectOfType<Emcee>();
        originalPosition = transform.position;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = originalPosition;
            currentRow = -1;
            ResetLoop();
        }

        if(Input.GetKeyDown(KeyCode.Space) && paused)
        {
            paused = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !paused)
        {
            paused = true;
        }

        if(!paused)
        {
            transform.Translate(Time.deltaTime * speed, 0, 0);
        }
    }

    
    void OnTriggerEnter(Collider target)
    {
        if(target.gameObject.tag.Equals("Block"))
        {
            // get cellblock from target collider
            CellBlock c = target.gameObject.GetComponent<CellBlock>();
            ToggleCellLight(c, true);

            // only play sound if this is a new row
            if (c.row > currentRow)
            {
                currentRow = c.row;
                Debug.Log(currentRow);

                // get all music blocks in current row and play simultaneously
                Block[] blocks = MC.getDataJockey().GetAllBlocksInRow(c.row);
                Block loop = CheckForLoop(blocks);

                if(loop != null)
                {
                    Debug.Log("Found a loop block!");

                    // IF the loop has been set to complete and this is a NEW loop block, start a new loop
                    if(loopCompleted && loop != loopTail && loop != loopHead)
                    {
                        ResetLoop();
                    }

                    if(loopTail == null)
                    {
                        loopTail = loop;
                    }

                    // the loop is only complete IF :
                    // current loop block is not loop head or tail
                
                    if(!loopCompleted && loop != loopTail && loopHead == null)
                    {
                        Debug.Log("Returning to loop tail...");
                        loopHead = loop;

                        // take the music wall back to position of previous loop block
                        transform.position = new Vector3(loopTail.x, transform.position.y, transform.position.z);
                        currentRow = -1;
                        // loop is now complete
                        loopCompleted = true;
                    }
                }

                PlayNotes(blocks);
            }
        }
    }

    void ResetLoop()
    {
        loopCompleted = false;
        loopTail = null;
        loopHead = null;
    }

    void OnTriggerExit(Collider target)
    {
        if (target.gameObject.tag.Equals("Block"))
        {
            CellBlock c = target.gameObject.GetComponent<CellBlock>();
            ToggleCellLight(c, false);
        }
    }

    // turn on light/emission when block is triggered
    void ToggleCellLight(CellBlock block, bool on)
    {
        Light light = block.GetComponent<Light>();
        light.enabled = on;

        Material mat = block.GetComponent<Renderer>().material;
        light.color = mat.color;

        if(on)
        {
            mat.EnableKeyword("_EMISSION");
        }
        else
        {
            mat.DisableKeyword("_EMISSION");
        }
    }

    // return an audio source that is not currently playing anything
    AudioSource GetAvailableSource()
    {
        foreach (AudioSource source in sources)
        {
            if (!source.isPlaying)
                return source;
        }
        return null;
    }

    void PlayNotes(Block[] blocks)
    {
        string audioPath;
        AudioClip[] clips = new AudioClip[blocks.Length];
        AudioSource source;

        for(int i = 0; i < blocks.Length; i++)
        {
            audioPath = blocks[i].getAudio();
            if(!audioPath.Equals("loop"))
            {
                clips[i] = Resources.Load<AudioClip>($"Audio/Etc/{audioPath}");
            }
        }

        foreach(AudioClip c in clips)
        {
            if(c != null)
            {
                Debug.Log($"Playing {c.name}!");
                source = GetAvailableSource();

                if (source != null)
                {
                    source.PlayOneShot(c);
                }
            }
        }
        
    }

    // iterate through row of blocks and check if a loop block exists
    // return the loop block
    Block CheckForLoop(Block[] blocks)
    {
        foreach(Block b in blocks)
        {
            if(b.isLoopBlock)
            {
                return b;
            }
        }
        return null;
    }
}
