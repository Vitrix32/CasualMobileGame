using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

/*
 * Name: PlayerMovement
 * Author: Michael Dabney & Isaac Drury
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

    [SerializeField]
    private Animator anim;

    private bool playerControl;
    private bool moving;
    private bool canMove;
    private Rigidbody2D rb;
    private Vector2 moveVector;

    //Start is called before the first frame update
    void Start()
    {
        playerControl = true;
        moving = false;
        canMove = true;
        rb = this.GetComponent<Rigidbody2D>();
        moveVector = Vector2.zero;
    }

    private void FixedUpdate()
    {
        moveLogic();
    }

    //Apply velocity to player
    private void moveLogic()
    {
        if (canMove || !playerControl)
        {
            Vector2 val = moveVector * moveSpeed * Time.fixedDeltaTime;
            rb.velocity = val;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        if (rb.velocity == Vector2.zero)
        {
            moving = false;
        }
        else
        {
            moving = true;
        }

        anim.SetFloat("horizontal", rb.velocity.x);
        anim.SetFloat("vertical", rb.velocity.y);
    }

    //Grab player input
    private void OnMovement(InputValue val)
    {
        moveVector = val.Get<Vector2>();
    }

    
    //This function has the purpose of setting the moveVector to any vector specified in the parameters.
    //This function allows other scripts to change the moveVector, such as the virtual joystick's script.
    public void setVector(Vector2 vec)
    {
        moveVector = vec;
    }

    
    //This function has the purpose of returning the value of the moving boolean.
    public bool isMoving()
    {
        return moving;
    }

    //This function has the purpose of enabling control of player movement apart from input
    public void StartDevControl()
    {
        playerControl = false;
    }

    //This function has the purpose of disabling control of player movement apart from input
    public void StopDevControl() 
    {
        playerControl = true;
    }
    
    //This function has the purpose of enabling player input to move the player character.
    public void EnableMovement()
    {
        canMove = true;
    }

    //This function has the purpose of disabling player input to move the player character.
    public void DisableMovement()
    {
        canMove = false;
    }
}
