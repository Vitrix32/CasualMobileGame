using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public bool switchScene = false;

    // Update is called once per frame
    void Update()
    {
        if (switchScene)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void ConditionMet()
    {
        switchScene = true;
    }

    public void returnHome()
    {
        SceneManager.LoadScene(0);
    }
}
