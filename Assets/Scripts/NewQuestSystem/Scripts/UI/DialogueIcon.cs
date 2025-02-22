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
    public void OnTempSubmit()
    {

    }

   /* void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("clicked");


            Vector2 mousePosition = Input.mousePosition;
            //Vector2 mouseScale = Input.mousePosition;
            //Ray ray = m_Camera.ScreenPointToRay(mousePosition);
            Vector3 cam = new Vector3(mousePosition.x, mousePosition.y, 10);

            RaycastHit2D hit = Physics2D.Raycast(m_Camera.transform.position, mousePosition);

            Debug.Log("hit " + hit.transform.gameObject.name);
            Debug.Log("this " + this.gameObject.name);
            
            if (hit.collider.gameObject == this.gameObject)
            {
                Debug.Log("you clicked the icon!");
            }
        }
    }*/

 

    public void OnMouseDown()
    {
        GameEventsManager.instance.dialogueEvents.EnterDialogue(knotName);

    }
}
