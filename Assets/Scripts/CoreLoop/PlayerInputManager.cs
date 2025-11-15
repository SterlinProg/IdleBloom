using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CoreLoop
{
    public class PlayerInputManager:MonoBehaviour,PCInputActions.IKeyboardActions
    {
        public PlayerInput playerInput;

        private void OnEnable()
        {
            InputActions = new PCInputActions();
        }

        public PCInputActions InputActions { get; set; }

        void Start()
        {
            playerInput.currentActionMap?.Enable();
            playerInput.currentActionMap = playerInput.currentActionMap;
            playerInput.defaultControlScheme = "Keyboard";
            InputActions.Enable();
            InputActions.Keyboard.SetCallbacks(this);
            InputActions.Disable();
            playerInput.actions = InputActions.asset;
            playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            // playerInput.uiInputModule = UnityEngine.EventSystems.EventSystem.current.GetComponent<InputSystemUIInputModule>();
            // ChangeToUIInputLayer();
        }

        public static EventHandler interacted;
        public static EventHandler started;

        public void OnInteract(InputAction.CallbackContext context)
        {
            if(!context.performed)
                return;
            interacted?.Invoke(this,EventArgs.Empty);
        }

        public void OnStart(InputAction.CallbackContext context)
        {
            if(!context.performed)
                return;
            started?.Invoke(this,EventArgs.Empty);
        }
    }
}