using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(InputProvider))]
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
    public bool isRigidbodyControl;
    public float height;
    public float groundCastDistance;
    public LayerMask groundMask;
    private Vector3 moveDirection;
    private Vector3 moveInput;

    public CharacterController characterController { get; private set; }
    public Rigidbody _rigidbody { get; private set; }


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
    Vector3 velocityRB;
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
    RaycastHit slopeHit;
    private void FixedUpdate()
    {
        if (isRigidbodyControl)
        {
            this.moveDirection = inputProvider.movementVector;
            float speed = moveSpeed;
            float acceleration =  accelarationGrounded;
            float accelerationY = (isHovering) ? accelarationHovering : acceleration;

            bool isGrounded = Physics.Raycast(transform.position + Vector3.up * height, Vector3.down, out slopeHit, height / 2 + groundCastDistance, groundMask);
            if (isGrounded)
            {
                float targetY = slopeHit.point.y + height;
                transform.DOMoveY(targetY, 0.1f);
                targetVelocityY = 0;
                //Debug.Log(targetY);
            }
            else
            {
                targetVelocityY += gravity * Time.deltaTime;
            }

            currentVelocityX = Mathf.SmoothDamp(currentVelocityX, moveDirection.x * speed, ref currentVelocityXRef, acceleration);
            currentVelocityZ = Mathf.SmoothDamp(currentVelocityZ, moveDirection.z * speed, ref currentVelocityZRef, acceleration);
            currentVelocityY = Mathf.SmoothDamp(currentVelocityY, targetVelocityY, ref currentVelocityYRef, accelerationY);

            velocityRB = new Vector3(currentVelocityX, targetVelocityY, currentVelocityZ);
            transform.Translate(velocityRB * Time.deltaTime);
            return;
        }
        else
        {
            if (!characterController.enabled)
                return;

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

    private void OnDrawGizmos()
    {
        if (isRigidbodyControl)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position + Vector3.up * height, transform.position + Vector3.up * height + Vector3.down * (height / 2 + groundCastDistance));
        }
    }
}
