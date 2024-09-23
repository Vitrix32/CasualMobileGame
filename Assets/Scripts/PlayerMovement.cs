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
    private GameObject background;
    private Rigidbody2D rb;
    private Vector2 moveVector;

    private float elapsedTime;
    private float fadeJoystick;
    private float fadeDuration;
    private bool inUse;
    private bool faded;

    // Start is called before the first frame update
    void Start()
    {
        knob = GameObject.Find("JoystickKnob");
        background = GameObject.Find("JoystickBackground");
        rb = this.GetComponent<Rigidbody2D>();
        moveVector = Vector2.zero;
        elapsedTime = 0.0f;
        fadeJoystick = 3.0f;
        fadeDuration = 0.5f;
        inUse = false;
        faded = false;
    }

    private void Update()
    {
        if (!inUse)
        {
            elapsedTime += Time.deltaTime;
        }
        if (!faded && (elapsedTime > fadeJoystick))
        {
            faded = true;
            knob.GetComponent<Fade>().StopAllCoroutines();
            knob.GetComponent<Fade>().startFade(0.1f, fadeDuration);
            background.GetComponent<Fade>().StopAllCoroutines();
            background.GetComponent<Fade>().startFade(0.1f, fadeDuration);
        }

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
            inUse = true;
            faded = false;
            elapsedTime = 0;
            knob.GetComponent<Fade>().StopAllCoroutines();
            knob.GetComponent<Fade>().startFade(0.7f, fadeDuration);
            background.GetComponent<Fade>().StopAllCoroutines();
            background.GetComponent<Fade>().startFade(0.7f, fadeDuration);
        }
    }

    private void HandleFingerUp(Finger playerFinger)
    {
        if (playerFinger == movementFinger)
        {
            movementFinger = null;
            knob.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            moveVector = Vector2.zero;
            inUse = false;
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
