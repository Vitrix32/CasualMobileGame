using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject WorldPlayer;
    private void Start()
    {
        WorldPlayer = GameObject.Find("WorldPlayer");
    }

    public void StartOver()
    {
        ResetSave();
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        WorldPlayer.GetComponent<PlayerStatus>().EnteringGameWorld(false, 0.4f);
        WorldPlayer.GetComponent<PlayerStatus>().SetWorldPosition();
        WorldPlayer.GetComponent<SpriteRenderer>().Equals(true);
        WorldPlayer.transform.position = new Vector3(243, -13, 0);
        SceneManager.LoadScene(PlayerPrefs.GetString("SceneName"));
        WorldPlayer.GetComponent<UniversalAudioHandling>().NewScene(PlayerPrefs.GetString("SceneName"));
        WorldPlayer.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void MainMenu()
    {    
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        WorldPlayer.GetComponent<PlayerStatus>().SetWorldPosition();
        Invoke("Menu", 0.4f);
    }

    // RESET the SAVE JSON files, then set the live JSON to be equal to the
    // RESET saved file
    private void ResetSave()
    {
        // RESETTING THE QUESTS
        if (File.Exists(Path.Combine(Application.persistentDataPath, "TestSaveQuests.txt")))
        {
            string tempJson = File.ReadAllText(Path.Combine(Application.persistentDataPath, "TestSaveQuests.txt"));
            QuestList questList = JsonUtility.FromJson<QuestList>(tempJson);
            List<Quest> list = new List<Quest>();
            for (int i = 0; i < questList.quests.Length; i++)
            {
                questList.quests[i].value = 0;
                list.Add(questList.quests[i]);
            }
            questList.quests = list.ToArray();
            string json = JsonUtility.ToJson(questList, true);
            // Reset the save and the live json files
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "TestSaveQuests.txt"), json);
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "Quests.txt"), json);
        }
        // RESETTING THE DIALOGUE
        if (File.Exists(Path.Combine(Application.persistentDataPath, "SaveDialogue.txt")))
        {
            string tempJson = File.ReadAllText(Path.Combine(Application.persistentDataPath, "SaveDialogue.txt"));
            NPCCollection npcList = JsonUtility.FromJson<NPCCollection>(tempJson);
            List<NPC> list = new List<NPC>();
            for (int i = 0; i < npcList.npc_characters.Length; i++)
            {
                npcList.npc_characters[i].value = 0;
                list.Add(npcList.npc_characters[i]);
            }
            npcList.npc_characters = list.ToArray();
            string json = JsonUtility.ToJson(npcList, true);
            // Reset the save and the live json files
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "SaveDialogue.txt"), json);
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "Dialogue.txt"), json);
        }
        // RESETTING THE PLAYERSTATS
        if (File.Exists(Path.Combine(Application.persistentDataPath, "SavePlayerStats.txt")))
        {
            string tempJson = File.ReadAllText(Path.Combine(Application.persistentDataPath, "SavePlayerStats.txt"));
            PlayerStats statList = JsonUtility.FromJson<PlayerStats>(tempJson);
            List<StatType> list = new List<StatType>();
            for (int i = 0; i < statList.stats.Length; i++)
            {
                statList.stats[i].itemEnhancement = 0;
                statList.stats[i].boostEnhancement = 0;
                statList.stats[i].itemName = "nothing";
                if (statList.stats[i].type == "attack")
                {
                    for (int j = 0; j < statList.stats[i].attackTypes.Count; j++)
                    {
                        statList.stats[i].attackTypes[j].itemEnhancement = 0;
                        statList.stats[i].attackTypes[j].boostEnhancement = 0;
                        statList.stats[i].attackTypes[j].itemName = "nothing";
                    }
                }
                list.Add(statList.stats[i]);
            }
            statList.stats = list.ToArray();
            string json = JsonUtility.ToJson(statList, true);
            // Reset the save and the live json files
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "SavePlayerStats.txt"), json);
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "PlayerStats.txt"), json);

            // PlayerPrefs.SetInt("Health", 50);
        }

        PlayerPrefs.SetFloat("XPos", 0);
        PlayerPrefs.SetFloat("YPos", 0);
        /*
         * LOCATION ID - KEY
         * Fill in as needed, if we need to have unique combat encounters. 
         * Change the location ID on trigger events based on where the player is
         * For example, change to desert when you walk into new biome, and back
         *          when you come back to Graville
         * 
         * Graville         - 0
         * Desert (South )  - 1
         * Dungeon          - 2
         * Dungeon Boss     - 3
         * 
         * 
         */
        PlayerPrefs.SetInt("LocID", 0);
        PlayerPrefs.SetString("SceneName", "GameplayScene");
        PlayerPrefs.SetInt("DunBoss", 0);
        PlayerPrefs.SetInt("FinBoss", 0);

        // Reset current health
        HealthManager.Instance.SetHealth(HealthManager.MAX_HEALTH);

        return;
    }

    public void QuitGame()
    {
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        Invoke("Quit", 0.4f);
    }

    private void Menu()
    {
        WorldPlayer.GetComponent<UniversalAudioHandling>().NewScene("MainMenu");
         
        SceneManager.LoadScene("MainMenu");
    }

    private void Play()
    {
        WorldPlayer.GetComponent<UniversalAudioHandling>().NewScene("GameplayScene");

        SceneManager.LoadScene("GameplayScene");
    }

    private void Quit()
    {
        Application.Quit();
    }
}