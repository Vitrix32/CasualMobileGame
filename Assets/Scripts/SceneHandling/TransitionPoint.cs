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
    private Vector2 destination;
    [SerializeField]
    private GameObject travelMsg;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] noises;

    private GameObject Player;
    private GameObject FadePanel;
    private GameObject PauseButton;
    private GameObject QuestButton;
    private GameObject Map;
    private GameObject VirtualJoystick;
    private int delay;
    private int index;

    private void Awake()
    {
        delay = 1;
    }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        FadePanel = GameObject.Find("FadePanel");
        PauseButton = GameObject.Find("PauseButton");
        QuestButton = GameObject.Find("OpenQuests");
        VirtualJoystick = GameObject.Find("JoystickPanel");

        Player.GetComponent<PlayerMovement>().EnableMovement();
        //Player.GetComponent<UniversalAudioHandling>().NewScene();
        travelMsg.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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
        if (audioSource != null && noises != null && noises.Length > 0)
        {
            index = Random.Range(0, noises.Length);
            audioSource.clip = noises[index];
            audioSource.Play();
        }
        FadePanel.GetComponent<SceneTransition>().End();
        travelMsg.SetActive(false);
        PauseButton.SetActive(false);
        QuestButton.SetActive(false);
        VirtualJoystick.SetActive(false);
        Player.GetComponent<PlayerMovement>().DisableMovement();
        Invoke("Leave", delay);
    }

    private void Leave()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        Player.transform.position = destination;
        FadePanel.GetComponent<SceneTransition>().Begin();
        travelMsg.SetActive(true);
        PauseButton.SetActive(true);
        QuestButton.SetActive(true);
        VirtualJoystick.SetActive(true);
        Player.GetComponent<PlayerMovement>().EnableMovement();
    }
    
}
