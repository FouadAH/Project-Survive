using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class GunController : MonoBehaviour
{
    public Transform gunModel;

    public Transform bulletSpawnTransform;
    public GameObject bulletPrefab; 

    public Transform targetObject;

    public LayerMask obstacleMask;

    public int maxBulletCount = 30;
    public int bulletFiredPerShot = 1;

    public float randomSpread;

    int currentBulletCount;

    public ShellSpawner ShellSpawner;
    public GameObject decalPrefab;

    public float reloadRate;
    public float reloadWaitTime = 2f;

    public float recoilTimeStart;
    public float recoilTimeEnd;

    public float recoilDistanceStart;
    public float recoilDistanceEnd;

    float lastFireTime;
    float lastReloadTime;
    float timeBeforeReload;

    //public TMPro.TMP_Text bulletCountText;
    public Transform bulletCountParent;
    public GameObject bulletCountPrefab;
    public LayerMask demagableMask;


    public DamageSource damageSource;

    private void Start()
    {
        currentBulletCount = maxBulletCount;
        for (int i = 1; i < maxBulletCount; i++)
        {
            Instantiate(bulletCountPrefab, bulletCountParent);
        }
    }

    private void Update()
    {
        Reload();
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

        for (int i = 0; i < bulletFiredPerShot; i++)
        {
            RaycastHit raycastHit;

            Quaternion randomRot = Quaternion.Euler(
            Random.Range(-randomSpread, randomSpread),
            Random.Range(-randomSpread, randomSpread),
            Random.Range(-randomSpread, randomSpread));

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
                    damagableBase.TakeDamage(damageSource.damageValue, -normal);
                }
            }

            var bullet = Instantiate(bulletPrefab, bulletSpawnTransform.position + characterController.velocity * 0.01f, Quaternion.identity);
            bullet.transform.forward = coneVector;
            bullet.GetComponent<BulletController>().targetPos = targetPos;
        }

        currentBulletCount--;
        lastFireTime = Time.time;
        timeBeforeReload = Time.time;
        lastReloadTime = 0;

        Destroy(bulletCountParent.GetChild(0).gameObject);

        Recoil();
        ShellSpawner.SpawnShell(gunModel, characterController.velocity);
    }

    void Recoil()
    {
        gunModel.DOLocalMoveX(recoilDistanceStart, recoilTimeStart).OnComplete(() =>
        {
            gunModel.DOLocalMoveX(recoilDistanceEnd, recoilTimeEnd);
        });
    }

    void Reload()
    {
        if (timeBeforeReload < lastFireTime + reloadWaitTime)
        {
            timeBeforeReload += Time.deltaTime;
            return;
        }

        if (currentBulletCount >= maxBulletCount)
        {
            return;
        }

        if (lastReloadTime > reloadRate)
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
