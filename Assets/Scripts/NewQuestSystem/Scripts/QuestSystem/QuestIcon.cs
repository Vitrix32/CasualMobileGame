using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    [Header("Icons")]
    [SerializeField] private GameObject requirementsNotMetToStartIcon;
    [SerializeField] private GameObject canStartIcon;
    [SerializeField] private GameObject requirementsNotMetToFinishIcon;
    [SerializeField] private GameObject canFinishIcon;
    private GameObject activeIcon;

    private void Start()
    {
        activeIcon = canStartIcon;
    }
    public void SetState(QuestState newState, bool startPoint, bool finishPoint)
    {
        // set all to inactive
        requirementsNotMetToStartIcon.SetActive(false);
        canStartIcon.SetActive(false);
        requirementsNotMetToFinishIcon.SetActive(false);
        canFinishIcon.SetActive(false);

        // set the appropriate one to active based on the new state
        switch (newState)
        {
            case QuestState.REQUIREMENTS_NOT_MET:
                if (startPoint) { activeIcon=requirementsNotMetToStartIcon; }
                break;
            case QuestState.CAN_START:
                if (startPoint) { activeIcon=canStartIcon; }
                break;
            case QuestState.IN_PROGRESS:
                if (finishPoint) {  activeIcon = requirementsNotMetToFinishIcon; }
                break;
            case QuestState.CAN_FINISH:
                if (finishPoint) { activeIcon = canFinishIcon; }
                break;
            case QuestState.FINISHED:
                break;
            default:
                Debug.LogWarning("Quest State not recognized by switch statement for quest icon: " + newState);
                break;
        }
    }

    public void show()
    {
        activeIcon?.SetActive(true);
        //canFinishIcon?.SetActive(true);//test
    }
    public void hide()
    {
        activeIcon?.SetActive(false);
    }

}
