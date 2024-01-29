using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ArcProjectile : ProjectileBase
{
    public LayerMask hitMask;
    [Range(1f, 2f)]
    public float speedModifier;

    public float angle;
    Ballistics.LaunchData launchData;
    Vector3 launchPosition;
    //public Transform endPos;
    private void Start()
    {
        //projectileRigidbody.useGravity = false;
        //Launch(endPos.position);
    }
    private void Update()
    {
        if (debugPath)
        {
            Ballistics.DrawPath(launchData, launchPosition);
        }
    }
    public override void Launch(Vector3 endPosition)
    {
        projectileRigidbody.useGravity = true;
        launchPosition = transform.position;

        float height = endPosition.y - transform.position.y;
        Vector3 displasementXZ = new Vector3(endPosition.x - launchPosition.x, 0, endPosition.z - launchPosition.z);

        maxHeight = Ballistics.HeightFromDistance(displasementXZ.magnitude, angle);

        if (height <= maxHeight)
        {
            launchData = Ballistics.CalculateVelocity(transform.position, endPosition, maxHeight);
        }
        else
        {
            Vector3 newTarget = endPosition;
            endPosition.y = maxHeight;

            launchData = Ballistics.CalculateVelocity(transform.position, newTarget, maxHeight);
        }
        projectileRigidbody.velocity = launchData.initialVelocity;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other != null)
        {
            if(Utility.IsInLayerMask(hitMask, other.gameObject.layer))
            {
                OnHit();
            }
        }
    }
}
