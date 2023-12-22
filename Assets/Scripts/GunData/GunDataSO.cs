using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunDataSO : ScriptableObject
{
    [Header("Bullet Settings")]

    public GameObject bulletPrefab;
    public int maxBulletCount = 30;
    public int bulletFiredPerShot = 1;

    public float randomSpreadRate;

    [Header("Reload Settings")]

    public float reloadRate;
    public float reloadWaitTime = 2f;

    [Header("Recoil Settings")]

    public float recoilTimeStart;
    public float recoilTimeEnd;

    public float recoilDistanceStart;
    public float recoilDistanceEnd;
}
