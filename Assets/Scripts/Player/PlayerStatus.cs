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

    public void EnteringCombat()
    {
        inCombat = true;
        worldPosition = this.transform.position;
        this.transform.position = combatPosition;
        this.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void ExitingCombat() 
    {
        inCombat = false;
        this.transform.position = worldPosition;
        this.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void EnteringNewArea()
    {
        worldPosition = this.transform.position;
    }

    public void ExitingNewArea()
    {
        this.transform.position = worldPosition;
    }

    public void EnableControl()
    {
        this.GetComponent<PlayerMovement>().enabled = true;
        rb.velocity = Vector2.zero;
    }

    public void DisableControl() 
    {
        this.GetComponent<PlayerMovement>().enabled = false;
        rb.velocity = Vector2.zero;
    }
}
