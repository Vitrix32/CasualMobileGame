using UnityEngine;

public class DesertChestMovement : MonoBehaviour
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
        if (dialogueManager.GetComponent<DialogueManager>().GetNPCValue("DesertChest") >= 1 && currentStep == 0)
        {
            currentStep++;
            // Start movement
            this.GetComponent<NPCMovement>().startPatrol();
        }
    }
}