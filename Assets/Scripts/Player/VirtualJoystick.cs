using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Joystick : MonoBehaviour
{
    [SerializeField]
    private Vector2 joystickSize;
    public GameObject player;

    private Finger movementFinger;
    private GameObject knob;
    private GameObject background;

    private float elapsedTime;
    private float fadeJoystick;
    private float fadeDuration;
    private bool inUse;
    private bool faded;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("WorldPlayer");
        knob = GameObject.Find("JoystickKnob");
        background = GameObject.Find("JoystickBackground");
    }

    // Update is called once per frame
    void Update()
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
        if (playerFinger != null && RectTransformUtility.RectangleContainsScreenPoint(this.GetComponent<RectTransform>(), playerFinger.screenPosition))
        {
            movementFinger = playerFinger;
            player.GetComponent<PlayerMovement>().setVector(Vector2.zero);
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
            player.GetComponent<PlayerMovement>().setVector(Vector2.zero);
            inUse = false;
        }
    }

    private void HandleFingerMove(Finger playerFinger)
    {
        if (playerFinger == movementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = joystickSize.x / 2.0f;

            if (Vector2.Distance(playerFinger.currentTouch.screenPosition, this.GetComponent<RectTransform>().anchoredPosition) > maxMovement)
            {
                knobPosition = (playerFinger.currentTouch.screenPosition - this.GetComponent<RectTransform>().anchoredPosition).normalized * maxMovement;
            }
            else
            {
                knobPosition = playerFinger.currentTouch.screenPosition - this.GetComponent<RectTransform>().anchoredPosition;
            }

            knob.GetComponent<RectTransform>().anchoredPosition = knobPosition;
            player.GetComponent<PlayerMovement>().setVector(knobPosition / maxMovement);
        }
    }
}
