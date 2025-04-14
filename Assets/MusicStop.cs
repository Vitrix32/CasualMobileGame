using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStop : MonoBehaviour
{
    public GameObject player = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        else
        {
            player.GetComponent<UniversalAudioHandling>().Die();
        }
    }

    public void KillAll()
    {
        Destroy(player);
        Destroy(GameObject.Find("DebugMenu"));
        Destroy(GameObject.Find("HealthManager"));
    }
}
