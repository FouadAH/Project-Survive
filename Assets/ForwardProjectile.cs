using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForwardProjectile : ProjectileBase
{
    public LayerMask hitMask;

    [Range(1f, 100f)]
    public float speedModifier;
    [Range(0f, 10f)]
    public float heightModifier;
    private Vector3 launchPosition;
    private bool hitLock;
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
        projectileRigidbody.velocity = Vector3.zero;
        endPosition = new Vector3(endPosition.x, endPosition.y + heightModifier, endPosition.z);
        Vector3 direction = endPosition - launchPosition;
        direction = Vector3.Normalize(direction);
        projectileRigidbody.AddForce(direction * speedModifier, ForceMode.Impulse);

    }

    public override void OnHit()
    {
        base.OnHit();
        ReturnToPool();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && !hitLock)
        {
            if (Utility.IsInLayerMask(hitMask, other.gameObject.layer))
            {
                OnHit();
            }
        }
    }
}
