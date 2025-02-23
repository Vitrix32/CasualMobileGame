using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private string questId;
    private int stepIndex;

    public void InitializeQuestStep(string questId, int stepIndex, string questStepState)
    {
        this.questId = questId;
        Debug.Log("ID: " + this.questId);
        this.stepIndex = stepIndex;
        if (questStepState != null && questStepState != "")
        {
            SetQuestStepState(questStepState);
        }
        GameEventsManager.instance.questEvents.QuestStepChange(questId, stepIndex);

    }

    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;
            Debug.Log("ID from QuestStep: "+this.questId);
            GameEventsManager.instance.questEvents.AdvanceQuest(this.questId);
            Destroy(this.gameObject);
        }
    }

    protected void ChangeState(string newState, string newStatus)
    {
        GameEventsManager.instance.questEvents.QuestStepStateChange(
            questId, 
            stepIndex, 
            new QuestStepState(newState, newStatus)
        );
    }

    public string getQuestIdOfStep()
    {
        return this.questId; 
    }

    protected abstract void SetQuestStepState(string state);
}
