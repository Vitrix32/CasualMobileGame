using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject target;
    private Vector2 TPLocation;
    public bool canTP;
    public GameObject player;

    void Start()
    {
        TPLocation = target.transform.position;
    }
    private void OnMouseDown()
    {
        if (canTP)
        {
            player.transform.position = TPLocation;
        }
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        canTP = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canTP = false;
    }
}
