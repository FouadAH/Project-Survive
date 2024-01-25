using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DamagableBase : MonoBehaviour
{
    public float health;
    public UnityEvent<Vector3, float> OnDeathEvent;

    public virtual void TakeDamage(float damageValue, Vector3 normal, float force = 1) 
    { 
        health -= damageValue;
        if(health <= 0)
        {
            OnDeath(normal, force);
        }
    }

    public virtual void Heal(float healValue)
    {
        health += healValue;
    }

    public virtual void OnDeath(Vector3 normal, float force = 1)
    {
        OnDeathEvent?.Invoke(normal, force);
    }
}
