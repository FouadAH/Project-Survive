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
    private void Start()
    {
        damageSource = GetComponent<DamageSource>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Hit()
    {
        //hasExploded = true;
        var colliders = Physics.OverlapSphere(transform.position, radius, damageable);

        //ParticleSystem particleSystem = Instantiate(explostionEffect, transform.position, Quaternion.identity);
        //particleSystem.Play();

        foreach (var collider in colliders.ToList().Distinct())
        {
            var damageController = collider.transform.root.GetComponent<DamageController>();
            if (damageController != null && damageSource != null)
            {
                damageController.TakeDamage(damageSource.damageValue, collider.transform.position - transform.position, force);
            }
        }

        Destroy(gameObject);
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
