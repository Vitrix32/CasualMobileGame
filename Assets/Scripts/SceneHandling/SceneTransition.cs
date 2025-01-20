using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    private bool end;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("WorldPlayer");
        end = false;
        this.GetComponent<Fade>().startFade(0.0f, 3.0f);
        this.transform.position = player.transform.position;
        Invoke("Reposition", 3.1f);
    }

    public void End()
    {
        end = true;
        Reposition();
        this.GetComponent<Fade>().startFade(1.0f, 3.0f);
    }

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
