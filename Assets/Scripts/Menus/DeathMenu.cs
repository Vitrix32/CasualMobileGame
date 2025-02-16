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
        WorldPlayer.transform.position = new Vector3(0, 0, 0);
        SceneManager.LoadScene(PlayerPrefs.GetString("SceneName"));
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
        if (File.Exists(Application.dataPath + "/Scripts/Dialogue/SaveQuests.txt"))
        {
            string tempJson = File.ReadAllText(Application.dataPath + "/Scripts/Dialogue/SaveQuests.txt");
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
            File.WriteAllText(Application.dataPath + "/Scripts/Dialogue/SaveQuests.txt", json);
            File.WriteAllText(Application.dataPath + "/Scripts/Dialogue/Quests.txt", json);
        }
        // RESETTING THE DIALOGUE
        if (File.Exists(Application.dataPath + "/Scripts/Dialogue/SaveDialogue.txt"))
        {
            string tempJson = File.ReadAllText(Application.dataPath + "/Scripts/Dialogue/SaveDialogue.txt");
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
            File.WriteAllText(Application.dataPath + "/Scripts/Dialogue/SaveDialogue.txt", json);
            File.WriteAllText(Application.dataPath + "/Scripts/Dialogue/Dialogue.txt", json);
        }
        // RESETTING THE PLAYERSTATS
        if (File.Exists(Application.dataPath + "/Scripts/Items/SavePlayerStats.txt"))
        {
            string tempJson = File.ReadAllText(Application.dataPath + "/Scripts/Items/SavePlayerStats.txt");
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
            File.WriteAllText(Application.dataPath + "/Scripts/Items/SavePlayerStats.txt", json);
            File.WriteAllText(Application.dataPath + "/Scripts/Items/PlayerStats.txt", json);

            PlayerPrefs.SetInt("Health", 50);
        }

        PlayerPrefs.SetFloat("XPos", 0);
        PlayerPrefs.SetFloat("YPos", 0);
        PlayerPrefs.SetString("SceneName", "GameplayScene");

        return;
    }

    public void QuitGame()
    {
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        Invoke("Quit", 0.4f);
    }

    private void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Play()
    {
        SceneManager.LoadScene("GameplayScene");
    }

    private void Quit()
    {
        Application.Quit();
    }
}