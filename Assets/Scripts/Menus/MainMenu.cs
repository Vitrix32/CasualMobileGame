using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject WorldPlayer;
    public GameObject LoadGameButton;
    public GameObject NewGameButton;
    public GameObject OptionsButton;
    public GameObject QuitButton;
    public string gameSceneName;
    private string dialoguePath;
    private string saveDialoguePath;
    private string questsPath;
    private string saveQuestsPath;
    private string playerStatsPath;
    private string savePlayerStatsPath;

    void Start()
    {
        WorldPlayer = GameObject.Find("WorldPlayer");
        Invoke("TurnButtonsOn", 3);

        // Initialize file paths with persistentDataPath
        dialoguePath = Path.Combine(Application.persistentDataPath, "Dialogue.txt");
        saveDialoguePath = Path.Combine(Application.persistentDataPath, "SaveDialogue.txt");
        questsPath = Path.Combine(Application.persistentDataPath, "Quests.txt");
        saveQuestsPath = Path.Combine(Application.persistentDataPath, "SaveQuests.txt");
        playerStatsPath = Path.Combine(Application.persistentDataPath, "PlayerStats.txt");
        savePlayerStatsPath = Path.Combine(Application.persistentDataPath, "SavePlayerStats.txt");

        //Debug.LogError("Backup: " + File.ReadAllText(Path.Combine(Application.persistentDataPath, "TestSaveQuests.txt")));

    }

    private void TurnButtonsOn()
    {
        LoadGameButton.SetActive(true);
        NewGameButton.SetActive(true);
        OptionsButton.SetActive(true);
        //QuitButton.SetActive(true);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(gameSceneName);
        PlayerPrefs.SetFloat("XPos", 243.0f);
        PlayerPrefs.SetFloat("YPos", -13.0f);
    }

    public void LoadGame()
    {
        SetLiveJSONToSave();

        PlayerPrefs.SetInt("LocID", PlayerPrefs.GetInt("SaveLocID"));


        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        WorldPlayer.GetComponent<PlayerStatus>().EnteringGameWorld(false, 0.4f);
        WorldPlayer.GetComponent<PlayerStatus>().SetWorldPosition();
        WorldPlayer.transform.position = new Vector3(PlayerPrefs.GetFloat("XPos"), PlayerPrefs.GetFloat("YPos"), 0);

        if ((int)HealthManager.Instance.GetHealth() <= 0)
        {
            HealthManager.Instance.SetHealth(10);
        }

        if (PlayerPrefs.HasKey("SceneName"))
        {
            string sceneName = PlayerPrefs.GetString("SceneName");
            WorldPlayer.GetComponent<UniversalAudioHandling>().NewScene(sceneName);
            SceneManager.LoadScene(sceneName);
            WorldPlayer.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            // No saved game found, load default scene
            SceneManager.LoadScene(gameSceneName);
            WorldPlayer.GetComponent<UniversalAudioHandling>().NewScene(gameSceneName);
            WorldPlayer.GetComponent<SpriteRenderer>().enabled = true;

        }
    }

    public void QuitGame()
    {
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        Invoke("Quit", 0.4f);
    }

    private void SetLiveJSONToSave()
    {
        try
        {
            // Load quest data
            if (File.Exists(saveQuestsPath))
            {
                //string quests = File.ReadAllText(saveQuestsPath);
                string quests = File.ReadAllText(Path.Combine(Application.persistentDataPath, "TestSaveQuests.txt"));
                File.WriteAllText(questsPath, quests);
                Debug.Log($"Quest data loaded successfully from {saveQuestsPath}");
                Debug.LogWarning(quests);
            }
            else
            {
                Debug.LogWarning($"SaveQuests.txt not found at {saveQuestsPath}");
            }

            // Load dialogue data
            if (File.Exists(saveDialoguePath))
            {
                string dialogue = File.ReadAllText(saveDialoguePath);
                File.WriteAllText(dialoguePath, dialogue);
                Debug.Log($"Dialogue data loaded successfully from {saveDialoguePath}");
            }
            else
            {
                Debug.LogWarning($"SaveDialogue.txt not found at {saveDialoguePath}");
            }

            // Load player stats
            if (File.Exists(savePlayerStatsPath))
            {
                string stats = File.ReadAllText(savePlayerStatsPath);
                File.WriteAllText(playerStatsPath, stats);
                Debug.Log($"Player stats loaded successfully from {savePlayerStatsPath}");
            }
            else
            {
                Debug.LogWarning($"SavePlayerStats.txt not found at {savePlayerStatsPath}");
            }

            // Restore health from backup
            HealthManager.Instance.RestoreFromBackup();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in SetLiveJSONToSave: {e.Message}\n{e.StackTrace}");
        }
    }

    private void Quit()
    {
        Debug.Log(Application.persistentDataPath);
        Application.Quit();
    }
}
