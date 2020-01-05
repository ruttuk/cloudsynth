using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DataJockey : MonoBehaviour
{
    [SerializeField]
    private Text newSongInput;

    Song _currentSong;
    string _currentSongName;

    public void CreateNewSong()
    {
        _currentSong = new Song();
        string newSongName = newSongInput.text;
        Debug.Log($"Creating new save file for {newSongName}...");
        newSongName = newSongName.Replace(' ', '_');
        SaveSong(newSongName);
        _currentSongName = newSongName;
    }

    private void SaveSong(string newSongName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + $"/{newSongName}.save");
        bf.Serialize(file, _currentSong);
        file.Close();
        Debug.Log("Game Saved!");
    }

    public Song LoadSong(string oldSongName)
    {
        if (File.Exists(Application.persistentDataPath + $"/{oldSongName}"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + $"/{oldSongName}", FileMode.Open);
            _currentSong = (Song)bf.Deserialize(file);
            file.Close();
            Debug.Log($"{oldSongName} Loaded!");
            _currentSongName = oldSongName.Substring(0, oldSongName.Length - 5);
        }
        else
        {
            Debug.Log($"{oldSongName} does not exist!!!");
        }
        return _currentSong;
    }

    public void UpdateSong(Block block, int row, int rowIndex, string action)
    {
        if (IsSongLoaded())
        {
            if (action.Equals("ADD_BLOCK"))
            {
                _currentSong.AddBlockToRow(block, row, rowIndex);
            }
            else if (action.Equals("DELETE_BLOCK"))
            {
                _currentSong.DeleteBlockFromRow(row, rowIndex);
            }
            SaveSong(_currentSongName);
        }
        else
        {
            Debug.Log("No song loaded!");
        }
    }

    public bool IsSongLoaded()
    {
        return _currentSong != null;
    }

    public Block[] GetAllBlocksInRow(int row)
    {
        Row r = _currentSong.GetRowByIndex(row);
        return r.getAllOccupiedBlocks();
    }
}
