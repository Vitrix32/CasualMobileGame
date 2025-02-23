using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTheMysticLake : QuestStep
{
    // Start is called before the first frame update 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(this.getQuestIdOfStep());
        if(collision.gameObject.CompareTag("Player"))
            FinishQuestStep();
    }

    protected override void SetQuestStepState(string state)
    {
        //things;
    }
}
