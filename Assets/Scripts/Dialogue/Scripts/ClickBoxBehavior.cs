using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickBoxBehavior : MonoBehaviour
{
    public GameObject manager;
    private DialogueManager DM;
    // Start is called before the first frame update
    void Start()
    {
        DM = manager.GetComponent<DialogueManager>();
    }

    private void OnMouseDown()
    {
        DM.click = true;
    }

    private void OnMouseUp()
    {
        DM.click = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
