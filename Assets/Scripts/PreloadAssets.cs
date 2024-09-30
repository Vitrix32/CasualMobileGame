using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadAssets : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(GameObject.Find("WorldPlayer"));
        DontDestroyOnLoad(GameObject.Find("RandomEncounter"));
        SceneManager.LoadScene("IsaacTestScene");
    }
}
