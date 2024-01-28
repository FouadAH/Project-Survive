using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallisticsTest : MonoBehaviour
{

    public Transform endPosition;
    //public float speed = 20f;
    //public float angle = 30f;
    public float maxHeight = 60f;

    public Rigidbody grenade;

    Ballistics.LaunchData launchData;
    Vector3 launchPosition;

    void Start()
    {
        grenade.useGravity = false;
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            grenade.useGravity = true;
            Launch();
        }

        Ballistics.DrawPath(launchData, launchPosition);
    }

    [ContextMenu("Launch")]
    void Launch()
    {
        launchPosition = transform.position;
        launchData = Ballistics.CalculateVelocity(transform.position, endPosition.position, maxHeight);
        grenade.velocity = launchData.initialVelocity;

        //float initialVelocity0 =  Ballistics.CalcualteInitialVelocityFromAngle(launchPosition, angle);
        //Vector3 launchVelocity = Ballistics.GetVelocityVector(initialVelocity0, angle);
        //float launchHeight = Ballistics.MaxHeight(launchVelocity);

        //launchData = Ballistics.CalculateVelocity(transform.position, endPosition.position, maxHeight);
        //grenade.velocity = launchData.initialVelocity;

        //Debug.Log($"Max Height: {launchHeight} -- Launch Velocity: {launchVelocity} -- Launch Angle: {angle}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1f);
        Gizmos.DrawWireSphere(endPosition.position, 1f);
    }
}
