using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cave : MonoBehaviour
{
    public void EnterCave()
    {
        PlayerPrefs.SetInt("LocID", 1);
    }

    public void ExitCave()
    {
        PlayerPrefs.SetInt("LocID", 0);
    }
}
