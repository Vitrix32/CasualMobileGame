using UnityEngine;
using UnityEngine.InputSystem;

// This script acts as a proxy for the PlayerInput component
// such that the input events the game needs to proces will 
// be sent through the GameEventManager. This lets any other
// script in the project easily subscribe to an input action
// without having to deal with the PlayerInput component directly.

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{


    
    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameEventsManager.instance.inputEvents.SubmitPressed();
        }
    }

    public void QuestLogTogglePressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameEventsManager.instance.inputEvents.QuestLogTogglePressed();
        }
    }
}
