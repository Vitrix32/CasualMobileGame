using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DialogueIcon : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button button;
    [SerializeField] private string knotName;
    public void OnMouseDown()
    {

        Debug.Log("selected");
        GameEventsManager.instance.dialogueEvents.EnterDialogue(knotName);
    }
}
