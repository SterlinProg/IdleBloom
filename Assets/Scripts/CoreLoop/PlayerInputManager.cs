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
        public static EventHandler<Vector2> mouseMoved;
        public static EventHandler<bool> pointerPressed;

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

        public void OnPointerDown(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                // Debug.Log(context.phase);
                // Debug.Log(Pointer.current.position.value);
                
                Ray pointerPos = Camera.main.ScreenPointToRay(Pointer.current.position.value);
                RaycastHit[] hits = Physics.RaycastAll(pointerPos,  Mathf.Infinity);
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out IPointerProcessor hitObj))
                    {
                        hitObj.ProcessTap(Pointer.current.position.value);
                    }
                }
                pointerPressed?.Invoke(this,true);
            }
            else if(context.canceled)
            {
                Ray pointerPos = Camera.main.ScreenPointToRay(Pointer.current.position.value);
                RaycastHit[] hits = Physics.RaycastAll(pointerPos,  Mathf.Infinity);
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out IPointerProcessor hitObj))
                    {
                        hitObj.ProcessRelease(Pointer.current.position.value);
                        // if(hitObj is IHoldable holdable && !holdable.TricklesDownAction())
                        //     return;
                    }
                }
                
                pointerPressed?.Invoke(this,false);
            }
        }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
            if(!context.performed)
                return;

            Vector3 newPos = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
            Debug.Log( newPos);
            mouseMoved?.Invoke(this,newPos );
        }
    }
}