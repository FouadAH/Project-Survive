using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBase : PooledObject
{
    protected Rigidbody projectileRigidbody;
    
    void Awake()
    {
        projectileRigidbody = GetComponent<Rigidbody>();
    }

    public virtual void Launch(Vector3 targetPosition) { }
    public virtual void OnHit() { }

    public virtual void OnEnable () { }
    public virtual void OnDisable() { }

}
