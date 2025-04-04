using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalBossEncounter : MonoBehaviour
{
    private float delay;
    private GameObject FadePanel;
    private GameObject PauseButton;
    private GameObject QuestButton;
    private GameObject MapButton;
    private GameObject VirtualJoystick;
    private int enemyId;
    private int numberOfIds;

    public GameObject FinalBoss;
    //public bool IsDungeonBossEncounter = false;

    private GameObject WorldPlayer;

    // Start is called before the first frame update
    void Start()
    {
        WorldPlayer = GameObject.Find("WorldPlayer");
        delay = 1;
        numberOfIds = 4;
        enemyId = Random.Range(1, numberOfIds);
        if (PlayerPrefs.GetInt("FinBoss") != 0)
        {
            FinalBoss.SetActive(false);
        }
        /*
        if (PlayerPrefs.GetInt("TempYVal") != 0)
        {
            WorldPlayer.transform.position = new Vector3((float)296.3, (float)83.3, WorldPlayer.transform.position.z);
            PlayerPrefs.SetInt("TempYVal", 0);
        }*/
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
                collision.gameObject.GetComponent<UniversalAudioHandling>().BossFight();
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
        PlayerPrefs.SetInt("LocID", 6);

        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("BattleScene");
    }
    private void SetObjectsToLiveJSON()
    {
        Debug.Log("Setting objects to live JSON (Encounter)");

        // Define paths using Application.persistentDataPath
        string questsPath = Path.Combine(Application.persistentDataPath, "Quests.txt");
        string dialoguePath = Path.Combine(Application.persistentDataPath, "Dialogue.txt");

        // QUESTS - Save current quest progress directly
        QuestList tempList = FindObjectOfType<QuestManager>()?.questList;
        if (tempList != null)
        {
            string json = JsonUtility.ToJson(tempList, true);
            File.WriteAllText(questsPath, json);
            Debug.Log($"Updated quests data at: {questsPath}");
        }
        else
        {
            Debug.LogError("Could not find QuestManager or questList is null");
        }

        // DIALOGUE - Access the npcData field directly
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager != null && dialogueManager.npcData != null)
        {
            string json = JsonUtility.ToJson(dialogueManager.npcData, true);
            File.WriteAllText(dialoguePath, json);
            Debug.Log($"Updated dialogue data at: {dialoguePath}");
        }
        else
        {
            Debug.LogError("Could not find DialogueManager or npcData is null");
        }
    }
}
