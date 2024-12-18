using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    private GameObject[] NPCs;
    private GameObject player;
    public TMPro.TextMeshProUGUI continueText;
    public GameObject textPanel;
    public TextAsset dialogue;
    private NPCCollection npcData;
    public TMPro.TextMeshProUGUI dialogueText;
    private bool waitForTextScroll = false;
    public bool click = false;
    public bool talking = false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("WorldPlayer");
        npcData = JsonUtility.FromJson<NPCCollection>(dialogue.text);
        NPCs = GameObject.FindGameObjectsWithTag("NPC");
    }

    private IEnumerator displayText(string s)
    {
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
            else
            {
                dialogueText.text += s[i];
                if (j >= 80 && s[i] == ' ')
                {
                    dialogueText.text += '\n';
                    j = 0;
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
        while (!waitForTextScroll)
        {
            click = false;
            yield return null;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                continueText.gameObject.SetActive(false);
                waitForTextScroll = true;
            }
        }
        textPanel.SetActive(false);
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
            if (npc.dialogue[npc.value].increment != "none")
            {
                NPC incNPC = FindNPCByName(npc.dialogue[npc.value].increment);
                incNPC.value++;
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

    private NPC FindNPCByName(string npcName)
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

<<<<<<< Updated upstream
    /*
    // Should be 3 files of classes
=======
    public void SaveNPCDialogue()
    {
        string json = JsonUtility.ToJson(npcData, true);
        File.WriteAllText(Application.dataPath + "/Scripts/Dilogue/NewDialogue.txt", json);
    }

>>>>>>> Stashed changes
    [System.Serializable]
    public class DialogueOption
    {
        public string option;
        public string increment;
    }

    [System.Serializable]
    public class NPC
    {
        public string name;
        public int value;
        public DialogueOption[] dialogue;
    }

    [System.Serializable]
    public class NPCCollection
    {
        public NPC[] npc_characters;
    }
    */
}
