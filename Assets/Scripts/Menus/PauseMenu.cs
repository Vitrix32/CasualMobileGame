using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject WorldPlayer;

    //public GameObject GameMenu;
    public GameObject pauseMenu;
    public GameObject pauseButton;
    public bool isPaused;
    
    // File paths
    private string dialoguePath;
    private string saveDialoguePath;
    private string questsPath;
    private string saveQuestsPath;
    private string playerStatsPath;
    private string savePlayerStatsPath;

    // Start is called before the first frame update
    void Start()
    {
        WorldPlayer = GameObject.Find("WorldPlayer");
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        
        // Initialize file paths
        dialoguePath = Path.Combine(Application.persistentDataPath, "Dialogue.txt");
        saveDialoguePath = Path.Combine(Application.persistentDataPath, "SaveDialogue.txt");
        questsPath = Path.Combine(Application.persistentDataPath, "Quests.txt");
        saveQuestsPath = Path.Combine(Application.persistentDataPath, "SaveQuests.txt");
        playerStatsPath = Path.Combine(Application.persistentDataPath, "PlayerStats.txt");
        savePlayerStatsPath = Path.Combine(Application.persistentDataPath, "SavePlayerStats.txt");
        
        // Ensure directories exist
        Directory.CreateDirectory(Path.GetDirectoryName(dialoguePath));
    }

    public void PauseGame()
    {
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        pauseButton.SetActive(false);
    }

    public void ResumeGame()
    {
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        pauseMenu.SetActive(false);
        //GameMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        pauseButton.SetActive(true);
        // Needs to change if we add menu music  --  DONT BE "EXITING COMBAT"
        
    }

    public void SaveAndExitToMenu()
    {
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        SaveGame();
        StartCoroutine(LoadMainMenuWithDelay());
    }

    public void DontSaveAndExitToMenu()
    {
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        StartCoroutine(LoadMainMenuWithDelay());
    }

    private IEnumerator LoadMainMenuWithDelay()
    {
        yield return new WaitForSecondsRealtime(0.4f);
        GoToMainMenu();
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("GoToMainMenu called");
        
        try 
        {
            string currentScene = SceneManager.GetActiveScene().name;
            string targetScene = "MainMenu";
            Debug.Log($"Current scene: {currentScene}, attempting to load: {targetScene}");
            WorldPlayer.GetComponent<UniversalAudioHandling>().NewScene(targetScene);
            // Try loading synchronously first as a test
            SceneManager.LoadScene(targetScene);
            

            Debug.Log("Scene load initiated");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load MainMenu scene: {e.Message}\nStack trace: {e.StackTrace}");
        }
    }

    public void SaveGame()
    {
        SetObjectsToLiveJSON();

        PlayerPrefs.SetInt("SaveLocID", PlayerPrefs.GetInt("LocID"));


        if (File.Exists(questsPath))
        {
            string quest = File.ReadAllText(questsPath);
            File.WriteAllText(saveQuestsPath, quest);
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "TestSaveQuests.txt"), quest);
        }

        if (File.Exists(dialoguePath))
        {
            string dialogue = File.ReadAllText(dialoguePath);
            File.WriteAllText(saveDialoguePath, dialogue);
        }

        if (File.Exists(playerStatsPath))
        {
            string stats = File.ReadAllText(playerStatsPath);
            File.WriteAllText(savePlayerStatsPath, stats);
        }

        PlayerPrefs.SetFloat("XPos", WorldPlayer.transform.position.x);
        PlayerPrefs.SetFloat("YPos", WorldPlayer.transform.position.y);
        PlayerPrefs.SetString("SceneName", SceneManager.GetActiveScene().name);
    }

    // THIS IS FOR QUEST AND DIALOGUE OBJECTS... NOT PLAYERSTATS
    private void SetObjectsToLiveJSON()
    {
        // QUESTS
        if (File.Exists(saveQuestsPath))
        {
            QuestList tempList = FindObjectOfType<QuestManager>().questList;

            string tempJson = File.ReadAllText(saveQuestsPath);
            QuestList questList = JsonUtility.FromJson<QuestList>(tempJson);
            List<Quest> list = new List<Quest>();

            for (int i = 0; i < questList.quests.Length; i++)
            {
                for (int j = 0; j < tempList.quests.Length; j++)
                {
                    if (tempList.quests[j].name == questList.quests[i].name)
                    {
                        questList.quests[i].value = tempList.quests[j].value;
                        break;
                    }
                }
                list.Add(questList.quests[i]);
            }
            questList.quests = list.ToArray();
            string json = JsonUtility.ToJson(questList, true);
            // Reset the save and the live json files
            File.WriteAllText(questsPath, json);
        }

        // DIALOGUE
        if (File.Exists(saveDialoguePath))
        {
            string tempJson = File.ReadAllText(dialoguePath);
            NPCCollection npcList = JsonUtility.FromJson<NPCCollection>(tempJson);
            List<NPC> list = new List<NPC>();
            for (int i = 0; i < npcList.npc_characters.Length; i++)
            {
                NPC tempNPC = FindObjectOfType<DialogueManager>().FindNPCByName(npcList.npc_characters[i].name);
                if (tempNPC == null)
                {
                    Debug.Log("ERROR: FIND NPC BY NAME RETURNED NULL - PAUSE MENU SCRIPT");
                }
                npcList.npc_characters[i].value = tempNPC.value;
                list.Add(npcList.npc_characters[i]);
            }
            npcList.npc_characters = list.ToArray();
            string json = JsonUtility.ToJson(npcList, true);
            File.WriteAllText(dialoguePath, json);
        }
    }

    public void QuitGame()
    {
        SaveGame();
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        Invoke("Quit", 0.4f);
    }

    private void Quit()
    {
        Application.Quit();
    }
}