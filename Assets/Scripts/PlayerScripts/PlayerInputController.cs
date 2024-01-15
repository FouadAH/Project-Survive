using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InputProvider))]
public class PlayerInputController : MonoBehaviour
{
    public Transform orientation;
    public PlayerRuntimeDataSO playerRuntimeDataSO;

    InputProvider inputProvider;
    Vector3 moveDirection;
    Vector3 normalizedInput;

    private void Awake()
    {
        inputProvider = GetComponent<InputProvider>();
    }

    private void Update()
    {
        playerRuntimeDataSO.playerPosition = transform.position;

        orientation.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        moveDirection = orientation.forward * normalizedInput.z + orientation.right * normalizedInput.x;

        inputProvider.OnMove(moveDirection);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        normalizedInput = context.ReadValue<Vector2>().normalized;
        normalizedInput = new Vector3(normalizedInput.x, 0, normalizedInput.y);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:

                if (context.interaction is HoldInteraction)
                {
                    inputProvider.OnStartHover();
                }
                break;

            case InputActionPhase.Canceled:

                inputProvider.OnStopHover();
                break;

            case InputActionPhase.Started:

                if (context.interaction is TapInteraction)
                {
                    inputProvider.OnJump();
                }
                break;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inputProvider.OnDash();
            inputProvider.OnStartSprintEvent();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            inputProvider.OnStopSprintEvent();
        }
    }
}
