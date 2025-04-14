using UnityEngine;

public class ThymeSoldierMovement : MonoBehaviour
{
    private GameObject dialogueManager;
    private int currentStep;

    void Start()
    {
        dialogueManager = this.GetComponent<NPCDialogue>().dialoguemanager;
        currentStep = 0;
    }

    void Update()
    {
        // Check for specific dialogue value
        if (dialogueManager.GetComponent<DialogueManager>().GetNPCValue("Jack") >= 3 && currentStep == 0)
        {
            currentStep++;
            // Start movement
            this.GetComponent<NPCMovement>().startPatrol();
            this.GetComponent<EdgeCollider2D>().enabled = false;
        }
    }
}