using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovementController : MonoBehaviour
{
    [Header("Speed Settings")]
    public float moveSpeed = 3f;

    [Header("Gravity Settings")]
    public float normalGravity = -50;
    float gravity = -3f;

    [Header("Acceleration Settings")]
    public float accelarationGrounded = 0.05f;
    public float accelarationHovering = 0.05f;
    public float accelarationAirborne = 0.05f;

    public float height;
    public float groundCastDistance;
    public LayerMask groundMask;

    private Vector3 moveDirection;

    float currentVelocityXRef;
    float currentVelocityX;

    float currentVelocityYRef;
    float currentVelocityY;
    float targetVelocityY;

    float currentVelocityZRef;
    float currentVelocityZ;

    bool isHovering;

    private InputProvider inputProvider;
    Vector3 velocityRB;
    RaycastHit slopeHit;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inputProvider = GetComponent<InputProvider>();

        gravity = normalGravity;
    }


    private void FixedUpdate()
    {
        moveDirection = inputProvider.movementVector;

        float speed = moveSpeed;
        float acceleration = accelarationGrounded;
        float accelerationY = (isHovering) ? accelarationHovering : acceleration;

        bool isGrounded = Physics.Raycast(transform.position + Vector3.up * height, Vector3.down, out slopeHit, height / 2 + groundCastDistance, groundMask);
        if (isGrounded)
        {
            float targetY = slopeHit.point.y + height;
            transform.DOMoveY(targetY, 0.1f);
            targetVelocityY = 0;
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position + Vector3.up * height, transform.position + Vector3.up * height + Vector3.down * (height / 2 + groundCastDistance));
    }
}
