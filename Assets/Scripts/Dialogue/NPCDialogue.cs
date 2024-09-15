using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public int currDialogue;
    public GameObject dialoguemanager;

    void OnMouseDown()
    {
        dialoguemanager.GetComponent<DialogueManager>().GetDialogue(this.name, currDialogue);
    }
}
