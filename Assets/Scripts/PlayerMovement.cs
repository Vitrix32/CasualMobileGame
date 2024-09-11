using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Vector2 moveVector;
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

    private void moveLogic()
    {
        Vector2 val = moveVector * moveSpeed * Time.fixedDeltaTime;
        Debug.Log(val);
        rb.velocity = val;
    }

    private void OnMovement(InputValue val)
    {
        moveVector = val.Get<Vector2>();
    }
}
