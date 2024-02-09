using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageProjectile : ArcProjectile
{
    public float radius;
    public float force;
    //public ParticleSystem explostionEffect;
    public LayerMask damageable;
    
    private DamageSource damageSource;
    private bool hasTriggeredHit;

    private MeshRenderer meshRenderer;
    private TrailRenderer trailRenderer;

    public override void OnEnable()
    {
        base.OnEnable();
        hasTriggeredHit = false;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        projectileRigidbody.drag = 0;

        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
    }

    private void Start()
    {
        damageSource = GetComponent<DamageSource>();
        meshRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }
   
    public void Hit()
    {
        var colliders = Physics.OverlapSphere(transform.position, radius, damageable);
        foreach (var collider in colliders.ToList().Distinct())
        {
            var damageController = collider.transform.root.GetComponent<DamageController>();
            if (damageController != null && damageSource != null)
            {
                damageController.TakeDamage(damageSource.damageValue, collider.transform.position - transform.position, force);
            }
        }

        ReturnToPool();
    }

    public override void Launch(Vector3 endPosition)
    {
        base.Launch(endPosition);
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true;
        }    
    }

    public override void OnHit()
    {
        base.OnHit();

        if (!hasTriggeredHit)
        {
            hasTriggeredHit = true;
            projectileRigidbody.drag = 10;
            Hit();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
