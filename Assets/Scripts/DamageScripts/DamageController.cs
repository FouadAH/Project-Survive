using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public void Init(float maxHealth)
    {
        health = maxHealth;
        InitMax(maxHealth);
    }
    public void InitMax(float maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.SetMax(maxHealth);
        }
    }
   
    public override void TakeDamage(float damageValue, Vector3 normal, float force = 1, DamageType damageType = DamageType.Range)
    {
        if (rb != null)
        {
            rb.AddForce(normal.normalized * damageValue * forceMod, ForceMode.Impulse);
        }

        base.TakeDamage(damageValue, normal, force, damageType);
    }
}
