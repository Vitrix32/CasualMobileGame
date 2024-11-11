using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private bool inCombat;
    private Vector2 combatPosition;
    private Vector2 worldPosition;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        inCombat = false;
        combatPosition = new Vector2(0.0f, 0.0f);
        worldPosition = Vector2.zero;
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void EnteringCombat()
    {
        inCombat = true;
        worldPosition = this.transform.position;
        this.transform.position = combatPosition;
        this.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void ExitingCombat() 
    {
        inCombat = false;
        this.transform.position = worldPosition;
        this.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void EnteringNewArea()
    {
        worldPosition = this.transform.position;
    }

    private void ExitingNewArea()
    {
        this.transform.position = worldPosition;
    }

    private void EnableControl()
    {
        this.GetComponent<PlayerMovement>().enabled = true;
        rb.velocity = Vector2.zero;
    }

    private void DisableControl() 
    {
        this.GetComponent<PlayerMovement>().enabled = false;
        rb.velocity = Vector2.zero;
    }

    /*****
    This function has the purpose of handling the transition of the player object from the game world to combat
    or new areas. you must specify if you are entering combat or just a new area, and a delay must be given.
    If no delay is desired just give a delay of 0.0f.
    *****/
    public void LeavingGameWorld(bool isCombat, float delay)
    {
        this.GetComponent<FootstepAudioHandling>().StopAllCoroutines();
        this.GetComponent<UniversalAudioHandling>().EnteringCombat();
        DisableControl();
        if (isCombat)
        {
            Invoke("EnteringCombat", delay);
        }
        else
        {
            Invoke("EnteringNewArea", delay);
        }
    }

    /*****
    This function has the purpose of handling the transition of the player object from the game world to combat or new areas.
    You must specify if you are entering combat or just a new area, and a delay must be given.
    If no delay is desired, just give a delay of 0.0f.
    *****/
    public void EnteringGameWorld(bool isCombat, float delay)
    {
        if (isCombat)
        {
            Invoke("ExitingCombat", delay);
        }
        else
        {
            Invoke("ExitingNewArea", delay);
        }
        EnableControl();
        this.GetComponent<UniversalAudioHandling>().ExitingCombat();
    }

    public void SetWorldPosition()
    {
        worldPosition = Vector2.zero;
    }
}
