using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : DamagableBase
{
    public float forceMod = 0.1f;
    public HealthSlider healthSlider;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (healthSlider != null)
        {
            healthSlider.SetMax(health);
        }
    }

    private void Update()
    {
        if (healthSlider != null)
        {
            healthSlider.sliderValue = health;
        }
    }

    public override void TakeDamage(float damageValue, Vector3 normal, float force = 1)
    {
        if (rb != null)
        {
            rb.AddForce(normal.normalized * damageValue * forceMod, ForceMode.Impulse);
        }

        base.TakeDamage(damageValue, normal, force);
    }
}
