using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : MonoBehaviour
{
    public GunDataSO gunDataSO;

    [Header("Spawn Points")]
    public Transform bulletSpawnTransform;
    public PhysicsParticleSpawner shellSpawner;

    [Header("Rig Target Points")]
    public Transform rightHandTarget;
    public Transform leftHandTarget;

    [Header("Canvas")]
    public Transform bulletCountParent;

}
