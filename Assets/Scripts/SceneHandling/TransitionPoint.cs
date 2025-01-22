using System.Collections;
using System.Collections.Generic;
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
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
            Debug.Log("Identified as player");
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
    //Includes starting the fade to black, disabling player control of movement
    public void Exiting()
    {
        travelMsg.SetActive(false);
        GameObject fadePanel = GameObject.Find("FadePanel");
        fadePanel.GetComponent<SceneTransition>().End();
        player.GetComponent<PlayerMovement>().DisableMovement();
        player.GetComponent<PlayerStatus>().SetPrevSceneIndex();
        StartCoroutine("Move", exitPoint.position);
        Invoke("Leave", 3.0f);
    }

    private IEnumerator Move(Vector3 dest)
    {
        Vector3 originalPosition = player.transform.position;
        float distance = Vector3.Distance(originalPosition, dest);
        float duration = distance / 3;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            player.transform.position = Vector2.Lerp(originalPosition, dest, time / duration);
            yield return null;
        }
    }

    private void Leave()
    {
        SceneManager.LoadScene(buildIndex);
    }
    
}
