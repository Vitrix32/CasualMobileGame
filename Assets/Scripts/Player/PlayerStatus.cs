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
        combatPosition = new Vector2(-3.0f, 2.0f); //Change to some different value later
        worldPosition = Vector2.zero;
        rb = this.GetComponent<Rigidbody2D>();
    }

    public void enteringCombat()
    {
        DontDestroyOnLoad(this.gameObject);
        inCombat = true;
        this.GetComponent<PlayerMovement>().enabled = false;
        rb.velocity = Vector2.zero;
        worldPosition = this.transform.position;
        this.transform.position = combatPosition;
    }

    public void exitingCombat() 
    {
        DontDestroyOnLoad(this.gameObject);
        inCombat = false;
        this.GetComponent<PlayerMovement>().enabled = false;
        rb.velocity = Vector2.zero;
        this.transform.position = worldPosition;
    }

    public void enteringNewArea()
    {
        DontDestroyOnLoad(this.gameObject);
        worldPosition = this.transform.position;
    }

    public void exitingNewArea()
    {
        DontDestroyOnLoad(this.gameObject);
        this.transform.position = worldPosition;
    }
}
