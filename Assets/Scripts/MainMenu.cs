using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup existingSongCanvasGroup;
    [SerializeField]
    private CanvasGroup newSongCanvasGroup;
    [SerializeField]
    private CanvasGroup mainButtonsCanvasGroup;
    [SerializeField]
    private CanvasGroup mainMenuCanvasGroup;
    [SerializeField]
    private CanvasGroup toolbeltCanvasGroup;
    [SerializeField]
    private GameObject existingSongButton;
    [SerializeField]
    private Transform spawnPoint;

    Emcee MC;

    private void Awake()
    {
        MC = Emcee.Instance;
    }

    bool _alreadyLoaded = false;

    public void ShowEditModeInterface()
    {
        ShowCanvasGroup(toolbeltCanvasGroup);
    }

    public void ShowInputField()
    {
        // hide main buttons and show input field
        HideCanvasGroup(mainButtonsCanvasGroup);
        ShowCanvasGroup(newSongCanvasGroup);
    }

    public void ShowMainMenu()
    {
        HideCanvasGroup(existingSongCanvasGroup);
        HideCanvasGroup(newSongCanvasGroup);
        ShowCanvasGroup(mainButtonsCanvasGroup);
    }

    public void HideMainMenu()
    {
        HideCanvasGroup(mainMenuCanvasGroup);
    }

    public void LoadSongs()
    {
        // hide main buttons and show existing song list
        HideCanvasGroup(mainButtonsCanvasGroup);
        ShowCanvasGroup(existingSongCanvasGroup);

        if (!_alreadyLoaded)
        {
            float spawnY;
            Vector3 spawnPos;
            GameObject spawnedButton;

            FileInfo[] SavedSongs = GetAllPersistentFiles();
            int numberOfExistingSongs = SavedSongs.Length;

            for (int i = 0; i < numberOfExistingSongs; i++)
            {
                string existingSongName = SavedSongs[i].Name;
                spawnY = i * 30;
                spawnPos = new Vector3(spawnPoint.position.x, -spawnY, spawnPoint.position.z);
                spawnedButton = Instantiate(existingSongButton, spawnPos, spawnPoint.rotation);
                spawnedButton.SetActive(true);
                spawnedButton.transform.SetParent(spawnPoint, false);
                // to get rid of the .save suffix
                spawnedButton.GetComponentInChildren<Text>().text = existingSongName.Substring(0, existingSongName.Length - 5);
                spawnedButton.GetComponentInChildren<Button>().onClick.AddListener(delegate { LoadExistingSong(existingSongName); });
            }
            _alreadyLoaded = true;
        }
    }

    private void LoadExistingSong(string existingSongName)
    {
        HideCanvasGroup(existingSongCanvasGroup);
        Song song = MC.getDataJockey().LoadSong(existingSongName);
        MC.getBlockManager().drawSong(song);
        ShowCanvasGroup(toolbeltCanvasGroup);
    }

    private FileInfo[] GetAllPersistentFiles()
    {
        DirectoryInfo d = new DirectoryInfo(Application.persistentDataPath);//Assuming Test is your Folder
        FileInfo[] Files = d.GetFiles("*.save"); //Getting Text files
        return Files;
    }

    private void HideCanvasGroup(CanvasGroup group)
    {
        group.alpha = 0f;
        group.blocksRaycasts = false;
    }

    private void ShowCanvasGroup(CanvasGroup group)
    {
        group.alpha = 1f;
        group.blocksRaycasts = true;
    }
}
