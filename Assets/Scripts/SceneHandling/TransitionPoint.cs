using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * Name: TransitionPoint.cs
 * Author: Isaac Drury
 * Date: January 2025
 * Description:
 * This file was created for the purpose of creating a point in the Gameworld that the player
 * could travel to other scenes from. It will be a component of a trigger in the game world.
 * Upon entering the trigger a prompt will appear to allow players to travel to other areas.
 * The intention is that the trigger and this accompanying script will be general-purpose enough
 * that it can be used for several parts of the gameworld. The player will automatically be
 * transported to the correct spot depending on which scene they came from before.
 */

public class TransitionPoint : MonoBehaviour
{
    [SerializeField]
    private int buildIndex;
    [SerializeField]
    private Transform exitPoint;
    [SerializeField]
    private Transform entryPoint;
    [SerializeField]
    private GameObject travelMsg;

    private GameObject joystick;
    private GameObject player;
    private int delay;

    private void Awake()
    {
        delay = 3;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        joystick = GameObject.Find("JoystickPanel");
        if (player.GetComponent<PlayerStatus>().GetPrevSceneIndex() == buildIndex ) 
        {
            player.transform.position = entryPoint.position;
            player.GetComponent<PlayerMovement>().EnableMovement();
            player.GetComponent<UniversalAudioHandling>().NewScene();
        }
        travelMsg.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TriggerEntered");
        if (other.gameObject.tag == "Player")
        {
            travelMsg.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            travelMsg.SetActive(false);
        }
    }

    //Handles details for exiting a scene and entering another
    public void Exiting()
    {
        travelMsg.SetActive(false);
        GameObject fadePanel = GameObject.Find("FadePanel");
        fadePanel.GetComponent<SceneTransition>().End();
        Vector3 moveVector = exitPoint.position - player.transform.position;
        moveVector.Normalize();
        Debug.Log(moveVector);
        joystick.SetActive(false);
        player.GetComponent<PlayerMovement>().setVector(moveVector);
        player.GetComponent<PlayerMovement>().StartDevControl();
        player.GetComponent<PlayerMovement>().DisableMovement();
        player.GetComponent<PlayerStatus>().SetPrevSceneIndex();
        Invoke("Leave", delay);
    }

    private void Leave()
    {
        SceneManager.LoadScene(buildIndex);
    }
    
}
