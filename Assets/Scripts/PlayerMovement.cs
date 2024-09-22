using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject joystick;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Vector2 joystickSize;

    private Finger movementFinger;
    private GameObject knob;
    private Rigidbody2D rb;
    private Vector2 moveVector;

    // Start is called before the first frame update
    void Start()
    {
        knob = GameObject.Find("JoystickKnob");
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

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleFingerUp;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleFingerUp;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void HandleFingerDown(Finger playerFinger)
    {
        if (playerFinger != null && RectTransformUtility.RectangleContainsScreenPoint(joystick.GetComponent<RectTransform>(), playerFinger.screenPosition))
        {
            movementFinger = playerFinger;
            moveVector = Vector2.zero;
        }
    }

    private void HandleFingerUp(Finger playerFinger)
    {
        if (playerFinger == movementFinger)
        {
            movementFinger = null;
            knob.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            moveVector = Vector2.zero;
        }
    }

    private void HandleFingerMove(Finger playerFinger)
    {
        if (playerFinger == movementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = joystickSize.x / 2.0f;

            if (Vector2.Distance(playerFinger.currentTouch.screenPosition, joystick.GetComponent<RectTransform>().anchoredPosition) > maxMovement)
            {
                knobPosition = (playerFinger.currentTouch.screenPosition - joystick.GetComponent<RectTransform>().anchoredPosition).normalized * maxMovement;
            }
            else
            {
                knobPosition = playerFinger.currentTouch.screenPosition - joystick.GetComponent<RectTransform>().anchoredPosition;
            }

            knob.GetComponent<RectTransform>().anchoredPosition = knobPosition;
            moveVector = knobPosition / maxMovement;
        }
    }
}
