using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunDataSO : ScriptableObject
{
    public float baseDamage;

    [Header("Bullet Settings")]

    public PooledObject bulletPrefab;
    public int maxBulletCount = 30;
    public int bulletFiredPerShot = 1;

    public float randomSpreadRate;

    [Header("Reload Settings")]

    public float reloadRate;
    public float reloadWaitTime = 2f;

    [Header("Recoil Settings")]

    public float recoilTimeStart;
    public float recoilTimeEnd;

    [Space(1)]

    public float recoilDistanceStartX;
    public float recoilDistanceEndX;

    [Space(1)]

    public float recoilDistanceStartY;
    public float recoilDistanceEndY;

    [Space(1)]

    public Vector3 recoilAngleStart = Vector3.zero;
    public Vector3 recoilAngleEnd = Vector3.zero;

    [Header("Fire Settings")]
    public bool canAutoFire;
    public bool isConeCast;
    public float fireRate;

    [Header("Data Settings")]
    public bool isAvailable;
    public bool isTripleShot;
    public bool isPentaShot;

    [Header("UI Settings")]
    public Color color = Color.white;

    public Action OnSetAvailable;

    private void OnEnable()
    {
        isAvailable = false;
    }

    public void SetIsAvailable()
    {
        isAvailable = true;
        OnSetAvailable?.Invoke();

    }
}
