using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    public TextAsset dialogue;
    private NPCCollection npcData;
    public TMPro.TextMeshProUGUI dialogueText;

    // Start is called before the first frame update
    void Start()
    {
        npcData = JsonUtility.FromJson<NPCCollection>(dialogue.text);
    }

    private IEnumerator displayText(string s)
    {
        dialogueText.text = "";
        for (int i = 0; i < s.Length; i++)
        {
            yield return new WaitForSeconds(.05f);
            dialogueText.text += s[i];
            if (i%80 == 79)
            {
                dialogueText.text += '\n';
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
            StartCoroutine(displayText(npc.dialogue[optionIndex].option));
        }
        StartCoroutine("Dialogue not found");
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
