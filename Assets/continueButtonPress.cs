using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class continueButtonPress : MonoBehaviour
{
    [SerializeField] private Button button;

    void Start()
    {
        button.onClick.AddListener(Clicked);
    }

    public void Clicked()
    {
        //Debug.Log("selected: "+eventData.selectedObject.name);
        //GameEventsManager.instance.dialogueEvents.SubmitChoice(choiceIndex);
        Debug.Log("Pressed");
        GameEventsManager.instance.inputEvents.SubmitPressed();
    }
}
