using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 moveVector;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        moveVector = Vector2.zero;
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        moveLogic();
    }

    //Apply velocity to player
    private void moveLogic()
    {
        Vector2 val = moveVector * moveSpeed * Time.fixedDeltaTime;
        rb.velocity = val;
    }

    //Grab player input
    private void OnMovement(InputValue val)
    {
        moveVector = val.Get<Vector2>();
    }

    private void OnInteract()
    {
        //do thing
    }
}
