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

    //public PlayerDataSO playerDataSO;
    public PlayerRuntimeDataSO playerRuntimeDataSO;
    public PlayerAbilityDataSO playerAbilityDataSO;
    public CharacterMovement characterMovement;
    [Range(0f, 1f)]
    public float rotationRate;
    InputProvider inputProvider;
    Vector3 moveDirection;
    Vector3 normalizedInput;

    private void Awake()
    {
        inputProvider = GetComponent<InputProvider>();
        characterMovement = GetComponent<CharacterMovement>();
        //health = playerDataSO.maxHealth;
    }

    private void Update()
    {
        playerRuntimeDataSO.playerMotion = transform.position - playerRuntimeDataSO.playerPosition;
        playerRuntimeDataSO.playerPosition = transform.position;

        orientation.rotation = Quaternion.Slerp(orientation.rotation, Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0), rotationRate);
        moveDirection = orientation.forward * normalizedInput.z + orientation.right * normalizedInput.x;

        inputProvider.OnMove(moveDirection);

        characterMovement.moveSpeed = playerAbilityDataSO.Speed;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        normalizedInput = context.ReadValue<Vector2>().normalized;
        normalizedInput = new Vector3(normalizedInput.x, 0, normalizedInput.y);
    }

    public void OnHeal(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:

                inputProvider.OnHeal_Start();
                break;

            case InputActionPhase.Canceled:
                inputProvider.OnHeal_Stop();
                break;
        }
    }

    public void OnKick(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:

                inputProvider.OnKick();
                break;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:

                if (context.interaction is HoldInteraction)
                {
                    if (!playerAbilityDataSO.hasHover)
                        return;

                    inputProvider.OnStartHover();
                }
                break;

            case InputActionPhase.Canceled:

                if (!playerAbilityDataSO.hasHover)
                    return;

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
        if (!playerAbilityDataSO.hasDash)
            return;

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
    public void OnDeath(Vector3 normal, float force = 1)
    {
    }

    public void TakeDamage(float damageValue, Vector3 normal, float force = 1)
    {

    }
}
