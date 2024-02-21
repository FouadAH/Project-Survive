using DG.Tweening;
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
    public Transform bulletCountPrefab;

    [Header("SFXs")]
    public AudioComponent reloadSFX;
    public AudioComponent fireSFX;

    [Header("Settings")]
    public bool isAvailable;

    private int currentBulletCount;

    private bool isRecoiling;

    private float lastFireTime;
    private float lastReloadTime;
    private float timeBeforeReload;

    private void Update()
    {
        Reload();
    }

    public void InitGun()
    {
        foreach (Transform child in bulletCountParent)
        {
            Destroy(child.gameObject);
        }

        currentBulletCount = gunDataSO.maxBulletCount;
        for (int i = 1; i < gunDataSO.maxBulletCount; i++)
        {
            Instantiate(bulletCountPrefab, bulletCountParent);
        }
    }

    public void OnFire()
    {
        currentBulletCount--;
        if (bulletCountParent.childCount > 0)
        {
            Destroy(bulletCountParent.GetChild(0).gameObject);
        }

        Recoil();

        Instantiate(fireSFX).PlaySFX();

        lastFireTime = Time.time;
        timeBeforeReload = 0;
    }

    public bool CanFire()
    {
        if (isRecoiling)
            return false;

        if (gunDataSO.canAutoFire)
        {
            if (Time.time < lastFireTime + gunDataSO.fireRate || currentBulletCount <= 0)
            {
                return false;
            }
        }

        if (currentBulletCount <= 0)
        {
            return false;
        }

        return true;
    }

    void Recoil()
    {
        if (isRecoiling)
            return;

        isRecoiling = true;
        transform.DOLocalMoveX(gunDataSO.recoilDistanceStartX, gunDataSO.recoilTimeStart).OnComplete(() =>
        {
            transform.DOLocalMoveX(gunDataSO.recoilDistanceEndX, gunDataSO.recoilTimeEnd).OnComplete(() => 
            {
                isRecoiling = false;
            });
        });

        transform.DOLocalMoveZ(gunDataSO.recoilDistanceStartY, gunDataSO.recoilTimeStart).OnComplete(() =>
        {
            transform.DOLocalMoveZ(gunDataSO.recoilDistanceEndY, gunDataSO.recoilTimeEnd);
        });

        transform.DOLocalRotate(gunDataSO.recoilAngleStart, gunDataSO.recoilTimeStart).OnComplete(() =>
        {
            transform.DOLocalRotate(gunDataSO.recoilAngleEnd, gunDataSO.recoilTimeEnd);
        });
    }

    void Reload()
    {
        if (timeBeforeReload < gunDataSO.reloadWaitTime)
        {
            timeBeforeReload += Time.deltaTime;
            return;
        }

        if (currentBulletCount >= gunDataSO.maxBulletCount)
        {
            return;
        }

        if (lastReloadTime > gunDataSO.reloadRate)
        {
            currentBulletCount++;
            lastReloadTime = 0;

            Instantiate(bulletCountPrefab, bulletCountParent);
            Instantiate(reloadSFX).PlaySFX();
        }
        else
        {
            lastReloadTime += Time.deltaTime;
        }
    }
}
