using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : MonoBehaviour
{
    public void EnterSnow()
    {
        PlayerPrefs.SetInt("LocID", 5);
    }

    public void ExitSnow()
    {
        PlayerPrefs.SetInt("LocID", 1);
    }
}
