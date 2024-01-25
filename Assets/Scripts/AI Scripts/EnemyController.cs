using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputProvider))]
public class EnemyController : Entity
{
    public PhysicsParticleSpawner particleSpawner;
    public override void Update()
    {
        base.Update();
    }

    public override void OnDeath(Vector3 damageDirection, float damageForce)
    {
        base.OnDeath(damageDirection, damageForce);

        if (isDead)
            return;

        particleSpawner.Spawn();
    }
}
