using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToJeffStep : QuestStep
{
    protected override void SetQuestStepState(string state)
    {
        Debug.Log("not implmented");
        state = "talk to jeff";
    }
}
