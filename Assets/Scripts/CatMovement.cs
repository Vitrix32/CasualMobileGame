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
 * related to how and when Cat should move are in this file.
 */

public class CatMovement : MonoBehaviour
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
        if (dialogueManager.GetComponent<DialogueManager>().GetNPCValue("Marlena") == 1 && currentStep == 0) //want this to trigger at Marlena == 1 then stop then go to next point at Cat == 2
        {
            currentStep++;
        }
        else if (currentStep == 1)
        {
            this.GetComponent<NPCMovement>().startPatrol();
            if (dialogueManager.GetComponent<DialogueManager>().GetNPCValue("Cat") == 2)
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
