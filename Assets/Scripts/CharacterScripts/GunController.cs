using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class GunController : MonoBehaviour
{
    public GunItem gunItem;

    public Transform bulletSpawnTransform;
    public Transform targetObject;

    public LayerMask obstacleMask;

    [Header("Effect Settings")]

    public ShellSpawner ShellSpawner;
    public GameObject decalPrefab;

    //public TMPro.TMP_Text bulletCountText;
    [Header("Canvas Settings")]
    public Transform bulletCountParent;
    public GameObject bulletCountPrefab;
    public LayerMask demagableMask;

    public float velocityMod;

    public DamageSource damageSource;

    int currentBulletCount;

    float lastFireTime;
    float lastReloadTime;
    float timeBeforeReload;

    private void Start()
    {
        currentBulletCount = gunItem.gunDataSO.maxBulletCount;
        for (int i = 1; i < gunItem.gunDataSO.maxBulletCount; i++)
        {
            Instantiate(bulletCountPrefab, bulletCountParent);
        }
    }

    private void Update()
    {
        Reload();
    }

    public void SetActiveGun(GunItem gunItem)
    {
        this.gunItem.gameObject.SetActive(false);
        this.gunItem = gunItem;
        this.gunItem.gameObject.SetActive(true);
    }

    public void Fire(CharacterController characterController)
    {
        if (currentBulletCount <= 0)
        {
            return;
        }

        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 projection = (Camera.main.transform.forward) * 100000f;
        Vector3 direction = projection - cameraPos;
        Vector3 targetPos = direction.normalized * 100000f;

        for (int i = 0; i < gunItem.gunDataSO.bulletFiredPerShot; i++)
        {
            RaycastHit raycastHit;

            Quaternion randomRot = Quaternion.Euler(
            Random.Range(-gunItem.gunDataSO.randomSpreadRate, gunItem.gunDataSO.randomSpreadRate),
            Random.Range(-gunItem.gunDataSO.randomSpreadRate, gunItem.gunDataSO.randomSpreadRate),
            Random.Range(-gunItem.gunDataSO.randomSpreadRate, gunItem.gunDataSO.randomSpreadRate));

            Vector3 coneVector = randomRot * Camera.main.transform.forward;
            var hit = Physics.Raycast(cameraPos, coneVector, out raycastHit, 100, demagableMask);

            if (hit)
            {
                Vector3 decalSpawnPos = raycastHit.point;
                Vector3 normal = raycastHit.normal;
                Quaternion quaternion = Quaternion.LookRotation(-normal, Vector3.up);

                var decal = Instantiate(decalPrefab, decalSpawnPos, quaternion);
                //float randomRotation = Random.Range(0, 180f);
                //decal.transform.Rotate(decal.transform.forward, randomRotation);
                decal.transform.parent = raycastHit.transform;

                DamagableBase damagableBase = raycastHit.collider.GetComponent<DamagableBase>();

                if (damagableBase != null)
                {
                    damagableBase.TakeDamage(damageSource.damageValue, -normal, 100);
                }
            }

            var bullet = Instantiate(gunItem.gunDataSO.bulletPrefab, bulletSpawnTransform.position + characterController.velocity * velocityMod, randomRot);
            bullet.transform.forward = coneVector;
            bullet.GetComponent<BulletController>().targetPos = targetPos;
            bullet.GetComponent<BulletController>().spawnTransform = bulletSpawnTransform;

        }

        currentBulletCount--;
        lastFireTime = Time.time;
        timeBeforeReload = Time.time;
        lastReloadTime = 0;

        Destroy(bulletCountParent.GetChild(0).gameObject);

        Recoil();
        ShellSpawner.SpawnShell(gunItem.transform, characterController.velocity);
    }

    void Recoil()
    {
        gunItem.transform.DOLocalMoveX(gunItem.gunDataSO.recoilDistanceStart, gunItem.gunDataSO.recoilTimeStart).OnComplete(() =>
        {
            gunItem.transform.DOLocalMoveX(gunItem.gunDataSO.recoilDistanceEnd, gunItem.gunDataSO.recoilTimeEnd);
        });
    }

    void Reload()
    {
        if (timeBeforeReload < lastFireTime + gunItem.gunDataSO.reloadWaitTime)
        {
            timeBeforeReload += Time.deltaTime;
            return;
        }

        if (currentBulletCount >= gunItem.gunDataSO.maxBulletCount)
        {
            return;
        }

        if (lastReloadTime > gunItem.gunDataSO.reloadRate)
        {
            currentBulletCount++;
            lastReloadTime = 0;

            Instantiate(bulletCountPrefab, bulletCountParent);
        }
        else
        {
            lastReloadTime += Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {

    }

}
