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
    [SerializeField]
    private Text TitleHeader;

    Emcee MC;

    private void Awake()
    {
        MC = Emcee.Instance;
    }

    // should be set to true after a song is exited
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
        HideCanvasGroup(toolbeltCanvasGroup);
        HideCanvasGroup(existingSongCanvasGroup);
        HideCanvasGroup(newSongCanvasGroup);
        ShowCanvasGroup(mainButtonsCanvasGroup);
        ShowCanvasGroup(mainMenuCanvasGroup);
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
            float yOffset = 30f;
            float spawnY = 0f;
            Vector3 spawnPos;
            GameObject spawnedButton;

            FileInfo[] SavedSongs = GetAllPersistentFiles();
            int numberOfExistingSongs = SavedSongs.Length;

            for (int i = 0; i < numberOfExistingSongs; i++)
            {
                string existingSongName = SavedSongs[i].Name;
                spawnPos = new Vector3(spawnPoint.position.x, -spawnY, spawnPoint.position.z);
                spawnedButton = Instantiate(existingSongButton, spawnPos, spawnPoint.rotation);
                spawnedButton.SetActive(true);
                spawnedButton.transform.SetParent(spawnPoint, false);
                // to get rid of the .save suffix
                spawnedButton.GetComponentInChildren<Text>().text = existingSongName.Substring(0, existingSongName.Length - 5);
                spawnedButton.GetComponentInChildren<Button>().onClick.AddListener(delegate { LoadExistingSong(existingSongName); });
                spawnY += yOffset;
            }
            _alreadyLoaded = true;
        }
    }

    public void ExitButtonPressed()
    {
        Debug.Log("Exiting...");
        // if exit button is pressed and main menu is loaded, quit the game.
        // otherwise quit the song and return to main menu.

        if (mainButtonsCanvasGroup.alpha == 1f)
        {
            Debug.Log("quitting game");
            Application.Quit();
        }
        else
        {
            Debug.Log("first erase all spawned cell blocks, unload song from data, then show main menu - also change title header");
            MC.getDataJockey().UnloadSong();
            MC.getBlockManager().CleanUpCellBlocks();
            ShowMainMenu();
            UnloadExistingSongs();
            TitleHeader.text = "cloudsynth";
        }
    }
    // when the song is exited, the existing songs in the main menu should be reloaded in case a new song has been saved
    private void UnloadExistingSongs()
    {
        int spawnedCells = spawnPoint.childCount;

        for (int i = 0; i < spawnedCells; i++)
        {
            Destroy(spawnPoint.GetChild(i).gameObject);
        }

        _alreadyLoaded = false;
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
