using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    private GameObject[] NPCs;
    public GameObject player;
    public TMPro.TextMeshProUGUI continueText;
    public GameObject textPanel;
    public GameObject clickBox;
    public TextAsset dialogue;
    public NPCCollection npcData;
    public TMPro.TextMeshProUGUI dialogueText;
    private bool waitForTextScroll = false;
    public bool click = false;
    public bool talking = false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("WorldPlayer");
        
        // Try to load from save file first, if it exists
        string path = Application.dataPath + "/Scripts/Dialogue/Dialogue.txt";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            if (!string.IsNullOrEmpty(json))
            {
                npcData = JsonUtility.FromJson<NPCCollection>(json);
                Debug.Log("Loaded dialogue from save file");
            }
            else
            {
                // Fallback to default dialogue
                npcData = JsonUtility.FromJson<NPCCollection>(dialogue.text);
                Debug.Log("Loaded default dialogue (save file was empty)");
            }
        }
        else
        {
            // No save file, use default
            npcData = JsonUtility.FromJson<NPCCollection>(dialogue.text);
            Debug.Log("Loaded default dialogue (no save file found)");
        }
        
        NPCs = GameObject.FindGameObjectsWithTag("NPC");
    }

    void OnEnable()
    {
        LoadDialogueProgress();
    }

    private IEnumerator displayText(string s)
    {
        clickBox.SetActive(true);
        talking = true;
        textPanel.SetActive(true);
        continueText.gameObject.SetActive(false);
        dialogueText.text = "";
        int j = 0;
        for (int i = 0; i < s.Length; i++)
        {
            j++;
            if (s[i] == '`')
            {
                j = 0;
                dialogueText.text += '\n';
                /*
                waitForTextScroll = false;
                while (!waitForTextScroll)
                {
                    click = false;
                    continueText.gameObject.SetActive(true);
                    yield return null;
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        continueText.gameObject.SetActive(false);
                        waitForTextScroll = true;
                    }
                }
                dialogueText.text = "";
                */
            }
            else
            {
                dialogueText.text += s[i];
                if (j >= 80 && s[i] == ' ')
                {
                    dialogueText.text += '\n';
                    j = 0;
                    /*
                    if (dialogueText.text.Length > 160)
                    {
                        waitForTextScroll = false;
                        while (!waitForTextScroll)
                        {
                            click = false;
                            continueText.gameObject.SetActive(true);
                            yield return null;
                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                continueText.gameObject.SetActive(false);
                                waitForTextScroll = true;
                            }
                        }
                        dialogueText.text = "";
                    }
                    */
                }
            }
            if (!click)
            {
                if (s[i] == '.' || s[i] == '?' || s[i] == '!')
                {
                    yield return new WaitForSeconds(.4f);
                }
                else
                {
                    yield return new WaitForSeconds(.05f);
                }
            }
        }
        waitForTextScroll = false;
        continueText.gameObject.SetActive(true);
        click = false;
        yield return new WaitForSeconds(.1f);
        while (!waitForTextScroll)
        {
            if (click)
            {
                float x = 0;
                while (x < .1f)
                {
                    x += Time.deltaTime;
                    if (!click) {
                        continueText.gameObject.SetActive(false);
                        waitForTextScroll = true;
                        break;
                    }
                    yield return null;
                }
            }
            yield return null;
        }
        textPanel.SetActive(false);
        clickBox.SetActive(false);
        player.GetComponent<PlayerStatus>().EndDialogue();
        talking = false;
        yield return null;
    }

    public void GetDialogue(string npcName)
    {
        NPC npc = FindNPCByName(npcName);
        int optionIndex = npc.value;

        StopAllCoroutines();
        if (npc != null && optionIndex >= 0 && optionIndex < npc.dialogue.Length)
        {
            this.GetComponent<QuestManager>().TryQuest(npcName + " " + optionIndex);

            string inc = npc.dialogue[npc.value].increment;

            if (inc != "none")
            {
                string[] increments = inc.Split(';');
                foreach (string incPart in increments)
                {
                    string trimmed = incPart.Trim();
                    string[] parts = trimmed.Split(' ');
                    if (parts.Length == 2)
                    {
                        NPC incNPC = FindNPCByName(parts[0]);
                        if (incNPC != null)
                        {
                            incNPC.value = int.Parse(parts[1]);
                        }
                    }
                }
                SaveDialogueProgress(); // Save when dialogue state changes
            }

            player.GetComponent<PlayerStatus>().BeginDialogue();
            StartCoroutine(displayText(npc.dialogue[optionIndex].option));
        }
    }


    //Finds NPC and 
    public int GetNPCValue(string npcName)
    {
        NPC npc = FindNPCByName(npcName);
        return npc.value;
    }

    public NPC FindNPCByName(string npcName)
    {
        foreach (NPC npc in npcData.npc_characters)
        {
            if (npc.name == npcName)
            {
                return npc;
            }
        }
        return null;
    }

    private void SaveDialogueProgress()
    {
        string path = Application.dataPath + "/Scripts/Dialogue/Dialogue.txt";
        string json = JsonUtility.ToJson(npcData, true);
        File.WriteAllText(path, json);
    }

    private void LoadDialogueProgress()
    {
        string path = Application.dataPath + "/Scripts/Dialogue/Dialogue.txt";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            NPCCollection savedProgress = JsonUtility.FromJson<NPCCollection>(json);
            
            // Restore dialogue progress
            foreach (NPC savedNPC in savedProgress.npc_characters)
            {
                NPC currentNPC = FindNPCByName(savedNPC.name);
                if (currentNPC != null)
                {
                    currentNPC.value = savedNPC.value;
                }
            }
        }
    }
}
