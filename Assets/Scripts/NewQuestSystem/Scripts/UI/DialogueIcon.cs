using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DialogueIcon : MonoBehaviour
{



    [SerializeField] private string knotName;
    private Camera m_Camera;

    void Start()
    {
        m_Camera = Camera.main;
    }
 
    
 

    public void OnMouseDown()
    {
        GameEventsManager.instance.dialogueEvents.EnterDialogue(knotName);

    }
}
