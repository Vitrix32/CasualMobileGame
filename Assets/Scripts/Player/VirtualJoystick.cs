using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Joystick : MonoBehaviour
{
    [SerializeField]
    private Vector2 joystickSize;
    [SerializeField]
    private GameObject knob;
    [SerializeField]
    private GameObject background;
    [SerializeField, Tooltip("If true, joystick will appear at touch position")]
    private bool dynamicJoystick = true;
    [SerializeField, Tooltip("Area where joystick can be activated (0-1 range of screen height)")]
    private float touchableAreaHeight = 0.7f;
    [SerializeField, Tooltip("Horizontal limit for joystick (0-1 range of screen width)")]
    private float touchableAreaWidth = 0.33f;
    [SerializeField, Tooltip("Time in seconds to determine if a touch is a tap or drag")]
    private float tapThreshold = 0.2f;
    [SerializeField, Tooltip("Distance in pixels to determine if a touch is a tap or drag")]
    private float dragThreshold = 10f;

    private Finger movementFinger;
    private GameObject player;
    private Vector2 touchStartPosition;
    private RectTransform joystickTransform;
    private Canvas parentCanvas;

    private float elapsedTime;
    private float fadeJoystick;
    private float fadeDuration;
    private bool inUse;
    private bool faded;
    
    // Variables for tap detection
    private float touchStartTime;
    private bool potentialTap;
    private Coroutine tapDetectionCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("WorldPlayer");
        elapsedTime = 0.0f;
        fadeJoystick = 1.0f;
        fadeDuration = 0.5f;
        inUse = false;
        faded = false;
        potentialTap = false;
        joystickTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
        
        // Initially hide the joystick if using dynamic mode
        if (dynamicJoystick)
        {
            SetJoystickAlpha(0f);
        }
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
        
        if (tapDetectionCoroutine != null)
        {
            StopCoroutine(tapDetectionCoroutine);
            tapDetectionCoroutine = null;
        }
    }

    private void HandleFingerDown(Finger playerFinger)
    {
        if (playerFinger == null)
            return;
            
        // Check if touch is in the lower part of the screen and left third
        bool validTouchArea = playerFinger.screenPosition.y < Screen.height * touchableAreaHeight && 
                              playerFinger.screenPosition.x < Screen.width * touchableAreaWidth;
        
        if (dynamicJoystick && validTouchArea)
        {
            // Record the starting information for tap detection
            touchStartPosition = playerFinger.screenPosition;
            touchStartTime = Time.time;
            potentialTap = true;
            
            // Start coroutine to wait for tap threshold before showing joystick
            if (tapDetectionCoroutine != null)
                StopCoroutine(tapDetectionCoroutine);
                
            tapDetectionCoroutine = StartCoroutine(DetectTapOrDrag(playerFinger));
        }
        else if (!dynamicJoystick)
        {
            // For static joystick, only respond to touches within left third if enabled
            if (playerFinger.screenPosition.x > Screen.width * touchableAreaWidth)
                return;
                
            // Original static joystick behavior
            if (RectTransformUtility.RectangleContainsScreenPoint(joystickTransform, playerFinger.screenPosition))
            {
                movementFinger = playerFinger;
                inUse = true;
                faded = false;
                elapsedTime = 0;
                knob.GetComponent<Fade>().StopAllCoroutines();
                knob.GetComponent<Fade>().startFade(0.7f, fadeDuration);
                background.GetComponent<Fade>().StopAllCoroutines();
                background.GetComponent<Fade>().startFade(0.7f, fadeDuration);
                player.GetComponent<PlayerMovement>().setVector(Vector2.zero);
            }
        }
    }

    private IEnumerator DetectTapOrDrag(Finger playerFinger)
    {
        // Wait for the tap threshold time
        yield return new WaitForSeconds(tapThreshold);
        
        // If finger is still down and still marked as potential tap, it's a drag
        if (playerFinger.isActive && potentialTap)
        {
            // Activate joystick as it's a drag
            movementFinger = playerFinger;
            PositionJoystickAtScreenPoint(touchStartPosition);
            
            // Make joystick visible
            SetJoystickAlpha(0.7f);
            inUse = true;
            faded = false;
            elapsedTime = 0;
            player.GetComponent<PlayerMovement>().setVector(Vector2.zero);
            
            potentialTap = false;
        }
        
        tapDetectionCoroutine = null;
    }

    private void HandleFingerUp(Finger playerFinger)
    {
        // Cancel any pending tap detection
        if (tapDetectionCoroutine != null)
        {
            StopCoroutine(tapDetectionCoroutine);
            tapDetectionCoroutine = null;
        }
        
        // Check if this was a tap (short touch within thresholds)
        if (potentialTap && playerFinger.isActive)
        {
            float touchDuration = Time.time - touchStartTime;
            float touchDistance = Vector2.Distance(touchStartPosition, playerFinger.screenPosition);
            
            if (touchDuration < tapThreshold && touchDistance < dragThreshold)
            {
                // This was a tap - don't activate joystick
                // You could trigger a tap event here if needed
                potentialTap = false;
                return;
            }
        }
        
        // Normal joystick cleanup
        if (playerFinger == movementFinger)
        {
            movementFinger = null;
            inUse = false;
            knob.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            player.GetComponent<PlayerMovement>().setVector(Vector2.zero);
            
            // Hide joystick if using dynamic mode
            if (dynamicJoystick)
            {
                SetJoystickAlpha(0f);
            }
        }
        
        potentialTap = false;
    }

    private void HandleFingerMove(Finger playerFinger)
    {
        // If we're still in potential tap mode and the finger has moved beyond threshold,
        // activate the joystick immediately
        if (potentialTap && playerFinger.isActive)
        {
            float touchDistance = Vector2.Distance(touchStartPosition, playerFinger.screenPosition);
            if (touchDistance > dragThreshold)
            {
                // Cancel the tap detection coroutine
                if (tapDetectionCoroutine != null)
                {
                    StopCoroutine(tapDetectionCoroutine);
                    tapDetectionCoroutine = null;
                }
                
                // Activate joystick immediately as it's definitely a drag
                movementFinger = playerFinger;
                PositionJoystickAtScreenPoint(touchStartPosition);
                
                // Make joystick visible
                SetJoystickAlpha(0.7f);
                inUse = true;
                faded = false;
                elapsedTime = 0;
                player.GetComponent<PlayerMovement>().setVector(Vector2.zero);
                
                potentialTap = false;
            }
        }
        
        // Normal joystick movement
        if (playerFinger == movementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = joystickSize.x / 2.0f;
            Vector2 currentPosition = dynamicJoystick ? touchStartPosition : (Vector2)joystickTransform.position;

            if (Vector2.Distance(playerFinger.currentTouch.screenPosition, currentPosition) > maxMovement)
            {
                knobPosition = (playerFinger.currentTouch.screenPosition - currentPosition).normalized * maxMovement;
            }
            else
            {
                knobPosition = playerFinger.currentTouch.screenPosition - currentPosition;
            }

            knob.GetComponent<RectTransform>().anchoredPosition = knobPosition;
            player.GetComponent<PlayerMovement>().setVector(knobPosition / maxMovement);
        }
    }
    
    private void PositionJoystickAtScreenPoint(Vector2 screenPoint)
    {
        // Constrain X position to stay within the left third of the screen
        float maxX = Screen.width * touchableAreaWidth;
        float halfJoystickWidth = joystickSize.x / 2.0f;
        
        // Make sure joystick stays fully visible
        float clampedX = Mathf.Clamp(screenPoint.x, halfJoystickWidth, maxX - halfJoystickWidth);
        Vector2 constrainedPosition = new Vector2(clampedX, screenPoint.y);
        
        if (parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            joystickTransform.position = constrainedPosition;
        }
        else
        {
            // For ScreenSpaceCamera or WorldSpace canvases
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.GetComponent<RectTransform>(),
                constrainedPosition,
                parentCanvas.worldCamera,
                out localPoint);
                
            joystickTransform.localPosition = localPoint;
        }
        
        // Reset knob position
        knob.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
    
    private void SetJoystickAlpha(float alpha)
    {
        // Skip fading and set alpha directly for immediate visibility changes
        // This is separate from the gradual fade effect during use
        Graphic[] graphics = background.GetComponentsInChildren<Graphic>();
        foreach (var graphic in graphics)
        {
            Color color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
        
        graphics = knob.GetComponentsInChildren<Graphic>();
        foreach (var graphic in graphics)
        {
            Color color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
    }
}
