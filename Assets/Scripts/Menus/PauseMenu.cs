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

    // Start is called before the first frame update
    void Start()
    {
        WorldPlayer = GameObject.Find("WorldPlayer");
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
    }

    void Update()
    {
        
    }

    public void PauseGame()
    {
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        pauseButton.SetActive(false);
        WorldPlayer.GetComponent<UniversalAudioHandling>().Pause();
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
        Debug.Log("SaveAndExitToMenu called");
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        SaveGame();
        StartCoroutine(LoadMainMenuWithDelay());
    }

    public void DontSaveAndExitToMenu()
    {
        Debug.Log("DontSaveAndExitToMenu called");
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        StartCoroutine(LoadMainMenuWithDelay());
    }

    private IEnumerator LoadMainMenuWithDelay()
    {
        Debug.Log("Starting delay before scene load");
        yield return new WaitForSecondsRealtime(0.4f);
        Debug.Log("Delay complete, calling GoToMainMenu");
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

    private void SaveGame()
    {
        SetObjectsToLiveJSON();

        string quest = File.ReadAllText(Application.dataPath + "/Scripts/Dialogue/Quests.txt");
        File.WriteAllText(Application.dataPath + "/Scripts/Dialogue/SaveQuests.txt", quest);

        string dialogue = File.ReadAllText(Application.dataPath + "/Scripts/Dialogue/Dialogue.txt");
        File.WriteAllText(Application.dataPath + "/Scripts/Dialogue/SaveDialogue.txt", dialogue);

        string stats = File.ReadAllText(Application.dataPath + "/Scripts/Items/PlayerStats.txt");
        File.WriteAllText(Application.dataPath + "/Scripts/Items/SavePlayerStats.txt", stats);

        PlayerPrefs.SetFloat("XPos", WorldPlayer.transform.position.x);
        PlayerPrefs.SetFloat("YPos", WorldPlayer.transform.position.y);
        PlayerPrefs.SetString("SceneName", SceneManager.GetActiveScene().name);
    }

    // THIS IS FOR QUEST AND DIALOGUE OBJECTS... NOT PLAYERSTATS
    private void SetObjectsToLiveJSON()
    {
        // QUESTS
        if (File.Exists(Application.dataPath + "/Scripts/Dialogue/SaveQuests.txt"))
        {


            QuestList tempList = FindObjectOfType<QuestManager>().questList;

            string tempJson = File.ReadAllText(Application.dataPath + "/Scripts/Dialogue/SaveQuests.txt");
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
            File.WriteAllText(Application.dataPath + "/Scripts/Dialogue/Quests.txt", json);
        }

        // DIALOGUE
        if (File.Exists(Application.dataPath + "/Scripts/Dialogue/SaveDialogue.txt"))
        {
            string tempJson = File.ReadAllText(Application.dataPath + "/Scripts/Dialogue/Dialogue.txt");
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
            File.WriteAllText(Application.dataPath + "/Scripts/Dialogue/Dialogue.txt", json);
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