using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetScenePosition : MonoBehaviour
{
    [SerializeField]
    private GameObject WorldPlayer;
    // Start is called before the first frame update
    void Start()
    {
        WorldPlayer = GameObject.Find("WorldPlayer");
        WorldPlayer.transform.position = new Vector3(PlayerPrefs.GetFloat("XPos"), PlayerPrefs.GetFloat("YPos"), 0);
    }
}
