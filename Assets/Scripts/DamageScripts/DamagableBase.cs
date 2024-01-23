using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamagableBase : MonoBehaviour
{
    public float health;
    public virtual void TakeDamage(float damageValue, Vector3 normal, float force = 1) 
    { 
        health -= damageValue;
        if(health <= 0)
        {
            OnDeath(normal, force);
        }
    }

    public virtual void OnDeath(Vector3 normal, float force = 1)
    {
        var entity = gameObject.GetComponent<Entity>();
        if(entity != null)
        {
            entity.OnDeath(normal, force);
        }
        else
        {
            Destroy(gameObject);
        }
        //Destroy(gameObject);
        //gameObject.GetComponent<Entity>()?.OnDeath();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
