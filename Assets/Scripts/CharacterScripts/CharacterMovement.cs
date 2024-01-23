using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(CharacterController), typeof(InputProvider))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Speed Settings")]
    public float moveSpeed = 3f;
    public float sprintSpeed = 18f;

    float gravity = -3f;

    [Header("Gravity Settings")]
    public float normalGravity = -50;

    [Header("Hover Settings")]
    public float hoverUpGravity = -10;
    public float hoverDownGravity = 10f;
    public float maxHoverDistance;

    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public float maxJumpModifier = 2f;

    [Header("Dash Settings")]
    public float dashForce = 10f;
    public float airDashForce = 10f;

    [Header("Acceleration Settings")]
    public float accelarationGrounded = 0.05f;
    public float accelarationHovering = 0.05f;
    public float accelarationAirborne = 0.05f;

    public Transform orientation;

    private Vector3 moveDirection;
    private Vector3 moveInput;

    public CharacterController characterController { get; private set; }

    float currentVelocityXRef;
    float currentVelocityX;

    float currentVelocityYRef;
    float currentVelocityY;
    float targetVelocityY;

    float currentVelocityZRef;
    float currentVelocityZ;

    Vector3 hoverStartPos;
    Coroutine hoverRoutine;

    float jumpHeldTime;
    bool isSprinting;
    bool isCrouched;
    bool isFalling;
    bool isRising;
    bool isHovering;

    private CinemachineImpulseSource impulseSource;
    private InputProvider inputProvider;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        characterController = GetComponent<CharacterController>();
        //impulseSource = GetComponent<CinemachineImpulseSource>();
        inputProvider = GetComponent<InputProvider>();

        gravity = normalGravity;
    }

    private void Start()
    {
        //inputProvider.MoveEvent += OnMove;
        inputProvider.DashEvent += OnDash;
        inputProvider.JumpEvent += OnJump;
        inputProvider.StartSprintEvent += OnStartSprinting;
        inputProvider.StopSprintEvent += OnStopSprinting;
        inputProvider.StartHoverEvent += OnStartHover;
        inputProvider.StopHoverEvent += OnStopHover;
    }

    private void OnDestroy()
    {
        //inputProvider.MoveEvent -= OnMove;
        inputProvider.DashEvent -= OnDash;
        inputProvider.JumpEvent -= OnJump;
        inputProvider.StartSprintEvent -= OnStartSprinting;
        inputProvider.StopSprintEvent -= OnStopSprinting;
        inputProvider.StartHoverEvent -= OnStartHover;
        inputProvider.StopHoverEvent -= OnStopHover;
    }

    private void FixedUpdate()
    {
        //orientation.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        //moveDirection = orientation.forward * moveInput.z + orientation.right * moveInput.x;
        this.moveDirection = inputProvider.movementVector;

        float speed = (isSprinting) ? sprintSpeed : moveSpeed;
        float acceleration = (characterController.isGrounded) ? accelarationGrounded : accelarationAirborne;
        float accelerationY = (isHovering) ? accelarationHovering : acceleration;

        currentVelocityX = Mathf.SmoothDamp(currentVelocityX, moveDirection.x * speed, ref currentVelocityXRef, acceleration);
        currentVelocityZ = Mathf.SmoothDamp(currentVelocityZ, moveDirection.z * speed, ref currentVelocityZRef, acceleration);
        currentVelocityY = Mathf.SmoothDamp(currentVelocityY, targetVelocityY, ref currentVelocityYRef, accelerationY);

        if (!characterController.isGrounded)
        {
            targetVelocityY += gravity * Time.deltaTime;
        }

        if (characterController.isGrounded && isHovering)
        {
            isHovering = false;
            gravity = normalGravity;
        }

        isFalling = currentVelocityY < 0 && !characterController.isGrounded;
        isRising = currentVelocityY > 0 && !characterController.isGrounded;

        characterController.Move(new Vector3(currentVelocityX, currentVelocityY, currentVelocityZ) * Time.deltaTime);
    }

    public void OnMove(Vector3 moveDirection)
    {
        this.moveDirection = moveDirection;
    }

    IEnumerator HoverRoutine()
    {
        hoverStartPos = transform.position;
        float distance = Mathf.Abs(hoverStartPos.y - transform.position.y);

        isHovering = true;
        gravity = hoverUpGravity;
        gravity = hoverUpGravity;

        while (distance < maxHoverDistance)
        {
            distance = Vector3.Distance(hoverStartPos, transform.position);
            yield return new WaitForFixedUpdate();
        }

        gravity = hoverDownGravity;
    }

    public void OnJump()
    {
        if (isSprinting)
            return;

        Jump();

        if (isHovering)
        {
            isHovering = false;
            gravity = normalGravity;
        }
        else if (isFalling && !isHovering)
        {
            isHovering = true;
            currentVelocityY = 0;
            targetVelocityY = 0;
            gravity = hoverDownGravity;
        }
    }

    public void OnDash()
    {
        Dash();
    }

    public void OnStartHover()
    {
        hoverRoutine = StartCoroutine(HoverRoutine());
    }

    public void OnStopHover()
    {
        if (isHovering)
        {
            StopCoroutine(hoverRoutine);
            currentVelocityY = 0;
            targetVelocityY = 0;
            gravity = hoverDownGravity;
        }
    }

    public void OnStartSprinting()
    {
        isSprinting = true;
    }

    public void OnStopSprinting()
    {
        isSprinting = false;
    }


    void Jump(float heightModifier = 1)
    {
        if (characterController.isGrounded)
        {
            targetVelocityY = jumpForce * heightModifier;
        }
    }

    void Dash()
    {
        if (characterController.isGrounded)
        {
            currentVelocityX = dashForce * moveDirection.x;
            currentVelocityZ = dashForce * moveDirection.z;
        }
        else
        {
            currentVelocityX = airDashForce * moveDirection.x;
            currentVelocityZ = airDashForce * moveDirection.z;
        }
    }
}
