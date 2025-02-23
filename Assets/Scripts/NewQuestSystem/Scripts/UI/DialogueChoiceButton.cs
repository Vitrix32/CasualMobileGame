using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DialogueChoiceButton : Button
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI choiceText;

    private int choiceIndex = -1;

    public void SetChoiceText(string choiceTextString)
    {
        choiceText.text = choiceTextString;
    }

    public void SetChoiceIndex(int choiceIndex)
    {
        this.choiceIndex = choiceIndex;
    }



    public void OnPointerDown()
    {
        GameEventsManager.instance.dialogueEvents.Choose(choiceIndex);
    }
}
