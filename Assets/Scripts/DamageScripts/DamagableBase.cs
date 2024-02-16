using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DamagableBase : MonoBehaviour
{
    public float health;
    [Header("Damage Events")]
    public UnityEvent<Vector3, float, DamageType> OnDeathEvent;
    public UnityEvent<float, Vector3,float, DamageType> OnTakeDamage;

    public virtual void TakeDamage(float damageValue, Vector3 normal, float force = 1, DamageType damageType = DamageType.Range) 
    {
        OnTakeDamage?.Invoke(damageValue, normal, force, damageType);

        health -= damageValue;
        if(health <= 0)
        {
            OnDeath(normal, force, damageType);
        }
    }

    public virtual void Heal(float healValue)
    {
        health += healValue;
    }

    public virtual void OnDeath(Vector3 normal, float force, DamageType damageType)
    {
        OnDeathEvent?.Invoke(normal, force, damageType);
    }
}

public enum DamageType
{
    None,
    Melee,
    Range,
}
