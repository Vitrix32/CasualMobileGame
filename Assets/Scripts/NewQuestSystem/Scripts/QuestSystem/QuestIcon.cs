using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    private Camera m_Camera;

    void Start()
    {
        m_Camera = Camera.main;
    }

    public void OnMouseDown()
    {
        //Debug.Log("clidcked");
        this.gameObject.GetComponentInParent<QuestPoint>()
            .IconClicked(GameEventsManager.instance.inputEvents.GetInputEventContext());
    }
}

