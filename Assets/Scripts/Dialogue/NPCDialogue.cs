using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public int currDialogue;
    public GameObject dialoguemanager;
    public GameObject questmanager;
    public GameObject talkImage;
    private bool canTalk = false;
    public GameObject[] increments;
    public GameObject[] questStarts;
    public GameObject[] questEnds;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            talkImage.SetActive(true);
            talkImage.transform.parent = this.transform;
            talkImage.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
            canTalk = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            talkImage.SetActive(false);
            canTalk = false;
        }
    }

    void OnMouseDown()
    {
        if (canTalk)
        {
            dialoguemanager.GetComponent<DialogueManager>().GetDialogue(this.name, currDialogue);
            if (currDialogue < increments.Length && increments[currDialogue] != null)
            {
                increments[currDialogue].gameObject.GetComponent<NPCDialogue>().currDialogue++;
                increments[currDialogue] = null;
            }
        }
        
    }
}
