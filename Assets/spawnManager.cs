using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{

    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("WorldPlayer");

        player.transform.position = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
