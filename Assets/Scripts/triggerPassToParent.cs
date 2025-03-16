using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerPassToParent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "WorldPlayer")
        {
            gameObject.GetComponentInParent<SceneMusicTest>().triggerEnter(gameObject.name); 
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "WorldPlayer")
            gameObject.GetComponentInParent<SceneMusicTest>().triggerExit(gameObject.name);
    }
}
