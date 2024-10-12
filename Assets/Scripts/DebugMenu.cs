using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DebugMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject sceneInput;
    private bool debugMode;
    private bool toggle1;
    private bool toggle2;
    private bool toggle3;

    // Start is called before the first frame update
    void Start()
    {
        debugMode = false;
        toggle1 = false;
        toggle2 = false;
        toggle3 = false;
    }

    //Toggles debug menu on and off
    //Use the = key to activate the debug menu in game
    private void OnDebug()
    {
        debugMode = !debugMode;
        if (debugMode)
        {
            this.gameObject.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            this.gameObject.GetComponent<Canvas>().enabled = false;
        }
    }

    //Takes the user to the scene specified in the search field
    //NO ERROR HANDLING - Make sure you enter the scene name in correctly
    public void goToScene()
    {
        SceneManager.LoadScene(sceneInput.GetComponent<TMP_InputField>().text);
    }

    //Toggles the time scale up and down
    public void toggleTimeScale()
    {
        toggle1 = !toggle1;
        if(toggle1)
        {
            Time.timeScale = 10;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    //Toggles the God-Mode on and off (makes player invincible and instakills enemies
    public void toggleGodMode()
    {
        toggle2 = !toggle2;
    }

    //Toggles player collision on and off
    public void toggleCollision()
    {
        toggle3 = !toggle3;
        if(toggle3) 
        { 
            player.GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            player.GetComponent<Collider2D>().enabled = true;
        }
    }

    //Getter for if God-Mode is active
    public bool inGodMode()
    {
        if (toggle2)
        {
            return true;
        }
        return false;
    }
}
