using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Name: SceneTransition
 * Author: Isaac Drury
 * Date: January 2025
 * Description:
 * The purpose of this script is to create a fade to/from black when exiting and
 * entering scenes respectively. It fades from black automatically at the start of 
 * the scene, and has a public method End() that can be called to fade to black.
 */

public class SceneTransition : MonoBehaviour
{
    private bool end;
    private GameObject player;
    private int delayDuration;

    //Start is called before the first frame update
    void Start()
    {
        delayDuration = 3;
        player = GameObject.Find("WorldPlayer");
        end = false;
        this.GetComponent<Fade>().startFade(0.0f, delayDuration);
        this.transform.position = player.transform.position;
        Invoke("Reposition", delayDuration);
    }

    //Makes scene fade to black
    public void End()
    {
        end = true;
        Reposition();
        this.GetComponent<Fade>().startFade(1.0f, delayDuration);
    }

    //Repositions fadepanel to prevent it from blocking other ui elements
    private void Reposition()
    {
        if (end)
        {
            this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
        }
        else
        {
            this.transform.position = new Vector3(this.transform.position.x - 10000, this.transform.position.y - 10000, this.transform.position.z);
        }
    }
}
