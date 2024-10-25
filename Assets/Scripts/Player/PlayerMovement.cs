using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

/*
 * Name: PlayerMovement
 * Author: Isaac Drury
 * Date: 9/22/24
 * Description:
 * This script was created for the purpose of applying movement to the
 * player character utilizing input gained from the Unity input system.
 * It applies physics to the player character using the Vector2 received
 * from the input. It also has functions supporting a virtual joystick
 * for touch screen usage.
 */
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    private bool moving;
    private Rigidbody2D rb;
    private Vector2 moveVector;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        moveVector = Vector2.zero;
        moving = false;
    }

    private void FixedUpdate()
    {
        moveLogic();
    }

    //Apply velocity to player
    private void moveLogic()
    {
        if (moveVector == Vector2.zero)
        {
            moving = false;
        }
        else
        {
            moving = true;
        }
        Vector2 val = moveVector * moveSpeed * Time.fixedDeltaTime;
        rb.velocity = val;
    }

    //Grab player input
    private void OnMovement(InputValue val)
    {
        moveVector = val.Get<Vector2>();
    }

    //Allows other scripts to change the moveVector, such as virtual joystick
    public void setVector(Vector2 vec)
    {
        moveVector = vec;
    }

    //
    public bool isMoving()
    {
        return moving;
    }
}
