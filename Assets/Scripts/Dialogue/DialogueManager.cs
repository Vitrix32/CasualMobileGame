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
            string inc = npc.dialogue[npc.value].increment;
            if (inc != "none")
            {
                string[] incData = inc.Split(' ');
                NPC incNPC = FindNPCByName(incData[0]);
                incNPC.value = int.Parse(incData[1]);
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
}
