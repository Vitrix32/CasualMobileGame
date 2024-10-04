using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    

    public TextAsset dialogue;
    private NPCCollection npcData;
    public TMPro.TextMeshProUGUI dialogueText;
    public Button endButton;
    public string endingDialogue;

    // Start is called before the first frame update
    void Start()
    {
        npcData = JsonUtility.FromJson<NPCCollection>(dialogue.text);
    }

    private IEnumerator displayText(string s)
    {
        dialogueText.text = "";
        int j = 0;
        for (int i = 0; i < s.Length; i++)
        {
            j++;
            dialogueText.text += s[i];
            if (j >= 80 && s[i] == ' ')
            {
                dialogueText.text += '\n';
                j = 0;
            }
            if (s[i] == '.' || s[i] == '?' || s[i] == '!')
            {
                yield return new WaitForSeconds(.4f);
            } else
            {
                yield return new WaitForSeconds(.05f);
            }
        }
        yield return null;
    }

    public void GetDialogue(string npcName, int optionIndex)
    {
        NPC npc = FindNPCByName(npcName);

        StopAllCoroutines();
        if (npc != null && optionIndex >= 0 && optionIndex < npc.dialogue.Length)
        {
            this.GetComponent<QuestManager>().TryQuest(npcName + " " + optionIndex);
            StartCoroutine(displayText(npc.dialogue[optionIndex].option));
            if (npcName + optionIndex == endingDialogue)
            {
                endButton.gameObject.SetActive(true);
            }
        }
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

    [System.Serializable]
    public class DialogueOption
    {
        public string option;
    }

    [System.Serializable]
    public class NPC
    {
        public string name;
        public DialogueOption[] dialogue;
    }

    [System.Serializable]
    public class NPCCollection
    {
        public NPC[] npc_characters;
    }
}
