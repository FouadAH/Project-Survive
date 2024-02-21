using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ArcProjectile : ProjectileBase
{
    public float maxHeight = 60f;
    public bool debugPath;

    public LayerMask hitMask;
    [Range(1f, 100f)]
    public float speedModifier;

    public float angle;
    Ballistics.LaunchData launchData;
    Vector3 launchPosition;
    bool hitLock;
    //public Transform endPos;
    public override void OnEnable()
    {
        base.OnEnable();
        hitLock = true;

        StartCoroutine(HitLock());
        projectileRigidbody.useGravity = false;
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    private void Update()
    {
        if (debugPath)
        {
            Ballistics.DrawPath(launchData, launchPosition);
        }

    }

    IEnumerator HitLock()
    {
        yield return new WaitForFixedUpdate();
        hitLock = false;
    }
    public override void Launch(Vector3 endPosition)
    {
        projectileRigidbody.useGravity = true;
        launchPosition = transform.position;
        projectileRigidbody.position = launchPosition;
        //projectileRigidbody.velocity = Vector3.zero;

        float height = endPosition.y - launchPosition.y;
        Vector3 displasementXZ = new Vector3(endPosition.x - launchPosition.x, 0, endPosition.z - launchPosition.z);
        maxHeight = Ballistics.HeightFromDistance(displasementXZ.magnitude, angle);


        //if (height <= maxHeight)
        //{
        //    launchData = Ballistics.CalculateVelocity(transform.position, endPosition, maxHeight);
        //}
        //else
        //{
        //    Vector3 newTarget = endPosition;
        //    endPosition.y = maxHeight;

        //    launchData = Ballistics.CalculateVelocity(transform.position, newTarget, maxHeight);
        //}
        launchData = Ballistics.CalculateVelocity(launchPosition, endPosition, maxHeight);

        if (!launchData.initialVelocity.IsNaN())
        {
            projectileRigidbody.velocity = launchData.initialVelocity + (endPosition - launchPosition).normalized * speedModifier;
        }
        //Debug.Log($" max-height: {maxHeight}, launchPosition: {launchPosition}, endPosition {endPosition}, distance: {displasementXZ.magnitude}");


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && !hitLock)
        {
            if(Utility.IsInLayerMask(hitMask, other.gameObject.layer))
            {
                OnHit();
            }
        }
    }
}
