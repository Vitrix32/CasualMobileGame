using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField]
    private int prevSceneIndex;
    [SerializeField]
    private bool combatImmunity;
    private bool inCombat;
    private Vector2 combatPosition;
    private Vector2 worldPosition;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        inCombat = false;
        combatImmunity = false;
        combatPosition = new Vector2(0.0f, 0.0f);
        worldPosition = Vector2.zero;
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void EnteringCombat()
    {
        inCombat = true;
        combatImmunity = true;
        worldPosition = this.transform.position;
        this.transform.position = combatPosition;
        this.GetComponent<PlayerMovement>().setVector(new Vector2(0, 0));
        ToggleSprite();
    }

    private void ExitingCombat()
    {
        inCombat = false;
        this.transform.position = worldPosition;
        Invoke("ToggleSprite", 0.01f);
    }

    //Enables player control of the player character
    private void EnableControl()
    {
        this.GetComponent<PlayerMovement>().EnableMovement();
    }

    //Disables player control of the player character
    private void DisableControl()
    {
        this.GetComponent<PlayerMovement>().DisableMovement();
    }

    //This function has the purpose of handling the transition of the player object from the game world to combat.
    //You must specify if you are entering combat or just a new area, and a delay must be given.
    //If no delay is desired just give a delay of 0.0f.
    public void LeavingGameWorld(bool isCombat, float delay)
    {
        this.GetComponent<FootstepAudioHandling>().StopAllCoroutines();
        this.GetComponent<UniversalAudioHandling>().EnteringCombat();
        DisableControl();
        Invoke("EnteringCombat", delay);
    }

    //This function has the purpose of handling the transition of the player object from the game world to combat or new areas.
    //You must specify if you are entering combat or just a new area, and a delay must be given.
    //If no delay is desired, just give a delay of 0.0f.
    public void EnteringGameWorld(bool isCombat, float delay)
    {
        // Same here probably, different parts of if statement to say which music to play
        // We could add a "isDeath" and a "isMenu" bool or something along those lines
        if (isCombat)
        {
            Invoke("ExitingCombat", delay);
        }
        EnableControl();
        Invoke("EndImmunity", 3.0f);
        this.GetComponent<UniversalAudioHandling>().ExitingCombat();
    }

    //This function has the purpose of disabling control of the player.
    //Will eventually hide various ui elements before dialogue.
    public void BeginDialogue()
    {
        DisableControl();
    }

    //This function has the purpose of reenabling control of the player. 
    //Will eventually reveal various ui elements after dialogue.
    public void EndDialogue()
    {
        EnableControl();
    }

    //This function has the purpose of setting the vector worldPosition to (0,0).
    public void SetWorldPosition()
    {
        worldPosition = Vector2.zero;
    }

    //This function allows other scripts to know the last scene the player was in.
    public int GetPrevSceneIndex()
    {
        return prevSceneIndex;
    }

    //This function allows other scripts to change the recorded last scene the player was in.
    public void SetPrevSceneIndex()
    {
        prevSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public bool IsCombatImmune()
    {
        return combatImmunity;
    }

    public void EnableImmunity()
    {
        combatImmunity = true;
    }
    private void EndImmunity()
    {
        combatImmunity = false;
    }

    private void ToggleSprite()
    {
        this.GetComponent<SpriteRenderer>().enabled = !this.GetComponent<SpriteRenderer>().enabled;
    }
}
