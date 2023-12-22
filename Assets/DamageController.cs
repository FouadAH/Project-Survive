using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : DamagableBase
{
    public float forceMod = 0.1f;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void TakeDamage(float damageValue, Vector3 normal)
    {
        if (rb != null)
        {
            rb.AddForce(normal.normalized * damageValue * forceMod, ForceMode.Impulse);
        }

        base.TakeDamage(damageValue, normal);
    }
}
