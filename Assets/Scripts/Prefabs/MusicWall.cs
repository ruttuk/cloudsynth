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

            // only play sound if this is a new row
            if(c.row > currentRow)
            {
                currentRow = c.row;
                Debug.Log(currentRow);
                // get all music blocks in current row and play simultaneously
                Block[] blocks = MC.getDataJockey().GetAllBlocksInRow(c.row);
                PlayNotes(blocks);
            }
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
            clips[i] = Resources.Load<AudioClip>($"Audio/Etc/{audioPath}");
        }

        foreach(AudioClip c in clips)
        {
            Debug.Log($"Playing {c.name}!");
            source = GetAvailableSource();

            if(source != null)
            {
                source.PlayOneShot(c);
            }
        }
        
    }
}
