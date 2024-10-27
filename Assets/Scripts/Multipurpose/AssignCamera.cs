using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        this.GetComponent<Canvas>().worldCamera = camera;
    }
}
