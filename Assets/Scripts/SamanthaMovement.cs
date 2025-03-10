using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Name: JeffMovement.cs
 * Author: Isaac Drury
 * Date: November 2024
 * Description:
 * This file will contain code specific to the movement logic of Jeff. It will utilize
 * the methods from the NPCMovement to achieve movement and idling, but the logic
 * related to how and when Samantha should move are in this file.
 */

public class SamanthaMovement : MonoBehaviour
{
    private GameObject dialogueManager;
    private int currentStep;
    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = this.GetComponent<NPCDialogue>().dialoguemanager;
        currentStep = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.GetComponent<DialogueManager>().GetNPCValue("Samantha") == 3 && currentStep == 0) //want this to trigger at Samantha == 1 then stop then go to next point at Samantha == 3
        {
            currentStep++;
        }
        else if (currentStep == 1)
        {
            this.GetComponent<NPCMovement>().startPatrol();
            if (dialogueManager.GetComponent<DialogueManager>().GetNPCValue("Samantha") == 3)
            {
                currentStep++;
            }
        }
        else if (currentStep == 2)
        {
            this.GetComponent<NPCMovement>().startPatrol();
        }
    }
}
