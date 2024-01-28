using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grenade : ArcProjectile
{
    public float radius;
    public float force;
    public float explosionDelay;
    private float currentTime;
    private float hitTime;
    public ParticleSystem explostionEffect;
    public LayerMask damageable;
    private DamageSource damageSource;
    private bool hasExploded;
    private bool hasTriggeredExplosion;

    private MeshRenderer meshRenderer;
    private void Start()
    {
        damageSource = GetComponent<DamageSource>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Explode()
    {
        hasExploded = true;
        var colliders = Physics.OverlapSphere(transform.position, radius, damageable);

        ParticleSystem particleSystem = Instantiate(explostionEffect, transform.position, Quaternion.identity);
        particleSystem.Play();

        foreach (var collider in colliders.ToList().Distinct())
        {
            Debug.Log("explode: " + collider.name);

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

        if (!hasTriggeredExplosion)
        {
            hasTriggeredExplosion = true;
            projectileRigidbody.drag = 10;
            StartCoroutine(ExplosionRoutine());
        }
    }

    IEnumerator ExplosionRoutine()
    {
        hitTime = Time.time;
        currentTime = Time.time;

        while (currentTime < hitTime + explosionDelay)
        {
            currentTime = Time.time;
            //float time = Mathf.Sin(currentTime);
            //meshRenderer.material.color = Color.Lerp(Color.white, Color.red, time);
            meshRenderer.material.color = Color.Lerp(Color.white, Color.red, currentTime / (hitTime + explosionDelay));

            yield return null;
        }

        Explode();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
