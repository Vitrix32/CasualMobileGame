using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    private bool end;
    // Start is called before the first frame update
    void Start()
    {
        end = false;
        this.GetComponent<Fade>().startFade(0.0f, 3.0f);
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
            this.transform.position = new Vector3(GameObject.Find("WorldPlayer").transform.position.x, GameObject.Find("WorldPlayer").transform.position.y, this.transform.position.z);
        }
        else
        {
            this.transform.position = new Vector3(this.transform.position.x - 10000, this.transform.position.y - 10000, this.transform.position.z);
        }
    }
}
