using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableBase : MonoBehaviour
{
    public float health;
    public virtual void TakeDamage(float damageValue, Vector3 normal) 
    { 
        health -= damageValue;
        if(health <= 0)
        {
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}
