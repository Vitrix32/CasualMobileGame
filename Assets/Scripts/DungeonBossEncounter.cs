using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonBossEncounter : MonoBehaviour
{
    private float delay;
    private GameObject FadePanel;
    private GameObject PauseButton;
    private GameObject QuestButton;
    private GameObject MapButton;
    private GameObject VirtualJoystick;
    private int enemyId;
    private int numberOfIds;

    public GameObject DungeonBoss;
    //public bool IsDungeonBossEncounter = false;

    private GameObject WorldPlayer;

    // Start is called before the first frame update
    void Start()
    {
        WorldPlayer = GameObject.Find("WorldPlayer");
        delay = 1;
        numberOfIds = 4;
        enemyId = Random.Range(1, numberOfIds);
        if (PlayerPrefs.GetInt("DunBoss") != 0)
        {
            DungeonBoss.SetActive(false);
        }
        if (PlayerPrefs.GetInt("TempYVal") != 0)
        {
            WorldPlayer.transform.position = new Vector3(WorldPlayer.transform.position.x, WorldPlayer.transform.position.y + PlayerPrefs.GetInt("TempYVal"), WorldPlayer.transform.position.z);
            PlayerPrefs.SetInt("TempYVal", 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Guaruntee combat, will disable object after combat
            int rand = Random.Range(0, 100);
            if(rand <= 100 && !collision.gameObject.GetComponent<PlayerStatus>().IsCombatImmune())
            {
                FadePanel = GameObject.Find("FadePanel");
                PauseButton = GameObject.Find("PauseButton");
                QuestButton = GameObject.Find("OpenQuests");
                MapButton = GameObject.Find("OpenMap");
                VirtualJoystick = GameObject.Find("JoystickPanel");
                FadePanel.GetComponent<SceneTransition>().End();
                PauseButton.SetActive(false);
                QuestButton.SetActive(false);
                MapButton.SetActive(false);
                VirtualJoystick.SetActive(false);
                collision.gameObject.GetComponent<PlayerStatus>().LeavingGameWorld(true, delay);
                StartCoroutine("ChangeScene");
            }
        }
    }

    private IEnumerator ChangeScene()
    {
        // Daniel -- Saving Fixes
        SetObjectsToLiveJSON();

        //IsDungeonBossEncounter = true;
        PlayerPrefs.SetInt("LocID", 3);

        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("BattleScene");
    }

    // Daniel -- Saving Fixes
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
}
