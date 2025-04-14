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
    public GameObject textPanel;
    public Scrollbar SB;
    public TextAsset dialogue;
    public NPCCollection npcData;
    public TMPro.TextMeshProUGUI dialogueText;
    private bool waitForTextScroll = false;
    public bool click = false;
    public bool talking = false;
    public PauseMenu pauseMenu;

    void Awake()
    {
        // Create directory if it doesn't exist
        string directory = Path.Combine(Application.persistentDataPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Check if dialogue file exists, if not copy from default
        string dialoguePath = Path.Combine(Application.persistentDataPath, "Dialogue.txt");
        if (!File.Exists(dialoguePath))
        {
            string defaultPath = Path.Combine(Application.dataPath, "Scripts/Dialogue", "SaveDialogue.txt");
            if (File.Exists(defaultPath))
            {
                File.Copy(defaultPath, dialoguePath);
                Debug.Log("Copied default dialogue to: " + dialoguePath);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("WorldPlayer");
        
        // Try to load from save file first, if it exists
        string path = Path.Combine(Application.persistentDataPath, "Dialogue.txt");
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

    public void setClick()
    {
        click = true;
    }

    private IEnumerator displayText(string s)
    {
        talking = true;
        textPanel.SetActive(true);
        SB.gameObject.SetActive(false);
        dialogueText.text = "";
        int j = 0;
        waitForTextScroll = false;
        click = false;
        int lines = 0;
        for (int i = 0; i < s.Length; i++)
        {
            j++;
            if (s[i] == '`')
            {
                Debug.Log(s[i]);
                j = 0;
                click = false;
                waitForTextScroll = false;

                while(!waitForTextScroll)
                {
                    if (click)
                    {
                        waitForTextScroll = true;
                    }
                    yield return null;
                }

                click = false;
                dialogueText.text = "";
                lines = 0;
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
                    lines++;
                    /*
                    if (lines == 4)
                    {
                        waitForTextScroll = false;
                        while (!waitForTextScroll)
                        {
                            click = false;
                            yield return null;
                            if (click)
                            {
                                waitForTextScroll = true;
                                lines = 0;
                            }
                        }
                        dialogueText.text = "";
                    }
                    */
                }
                if (lines > 4)
                {
                    SB.gameObject.SetActive(true);
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
        click = false;
        yield return new WaitForSeconds(.1f);
        while (!waitForTextScroll)
        {
            if (click)
            {
                waitForTextScroll = true;
            }
            yield return null;
        }
        textPanel.SetActive(false);
        player.GetComponent<PlayerStatus>().EndDialogue();
        talking = false;
        yield return null;
    }

    public void GetDialogue(string npcName)
    {
        if (pauseMenu.isPaused)
        {
            return;
        }
        Debug.Log(npcName);
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
        string path = Path.Combine(Application.persistentDataPath, "Dialogue.txt");
        string json = JsonUtility.ToJson(npcData, true);
        File.WriteAllText(path, json);
    }

    private void LoadDialogueProgress()
    {
        string path = Path.Combine(Application.persistentDataPath, "Dialogue.txt");
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
