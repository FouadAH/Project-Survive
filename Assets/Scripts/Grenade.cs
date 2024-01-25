using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float radius;
    public float force;
    public LayerMask damageable;
    private DamageSource damageSource;

    private void Start()
    {
        damageSource = GetComponent<DamageSource>();
    }

    [ContextMenu("Explode")]
    public void Explode()
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
