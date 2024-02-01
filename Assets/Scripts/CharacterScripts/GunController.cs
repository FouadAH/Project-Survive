using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Animations.Rigging;

public class GunController : MonoBehaviour
{
    public GunItem gunItem;
    public PlayerAbilityDataSO playerAbilityDataSO;
    public Transform targetObject;
    public LayerMask obstacleMask;
    public Collider coneCollider;

    [Header("Effect Settings")]
    public GameObject decalPrefab;
    public AudioComponent fireSFX;
    public AudioComponent reloadSFX;

    [Header("IK Settings")]
    public RigBuilder rigBuilder;
    public TwoBoneIKConstraint leftHandIK;
    public TwoBoneIKConstraint rightHandIK;

    [Header("Canvas Settings")]
    public CrosshairController crosshairController;
    public GameObject bulletCountPrefab;
    public LayerMask demagableMask;

    public float velocityMod;
    public bool isHeld;
    public DamageSource damageSource;
    CharacterController characterController;
    int currentBulletCount;

    float lastFireTime;
    float lastReloadTime;
    float timeBeforeReload;

    private void Start()
    {
        InitCanvas();
    }

    private void Update()
    {
        if (gunItem == null)
            return;

        Reload();
        if(isHeld && gunItem.gunDataSO.canAutoFire)
        {
            Fire(characterController);
        }

        damageSource.damageValue = gunItem.gunDataSO.baseDamage + playerAbilityDataSO.damageMod;
    }

    public void SwitchActiveGun(GunItem gunItem)
    {
        this.gunItem.gameObject.SetActive(false);
        this.gunItem = gunItem;
        this.gunItem.gameObject.SetActive(true);
        InitCanvas();

        leftHandIK.data.target = gunItem.leftHandTarget;
        rightHandIK.data.target = gunItem.rightHandTarget;
        rigBuilder.Build();
    }

    public void InitCanvas()
    {
        foreach (Transform child in gunItem.bulletCountParent)
        {
            Destroy(child.gameObject);
        }

        currentBulletCount = gunItem.gunDataSO.maxBulletCount;
        for (int i = 1; i < gunItem.gunDataSO.maxBulletCount; i++)
        {
            Instantiate(bulletCountPrefab, gunItem.bulletCountParent);
        }
    }

    public void Fire(CharacterController characterController)
    {
        this.characterController = characterController;

        if (gunItem.gunDataSO.canAutoFire)
        {
            if (Time.time < lastFireTime + gunItem.gunDataSO.fireRate)
            {
                return;
            }
        }


        if (currentBulletCount <= 0)
        {
            return;
        }

        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 projection = (Camera.main.transform.forward) * 100000f;
        Vector3 direction = projection - cameraPos;
        Vector3 targetPos = direction.normalized * 100000f;

        if (gunItem.gunDataSO.isConeCast)
        {
            StartCoroutine(ConeCast());
        }

        for (int i = 0; i < gunItem.gunDataSO.bulletFiredPerShot; i++)
        {
            RaycastHit raycastHit;

            Quaternion randomRot = Quaternion.Euler(
            Random.Range(-gunItem.gunDataSO.randomSpreadRate, gunItem.gunDataSO.randomSpreadRate),
            Random.Range(-gunItem.gunDataSO.randomSpreadRate, gunItem.gunDataSO.randomSpreadRate),
            Random.Range(-gunItem.gunDataSO.randomSpreadRate, gunItem.gunDataSO.randomSpreadRate));

            Vector3 coneVector = randomRot * Camera.main.transform.forward;

            if (!gunItem.gunDataSO.isConeCast)
            {
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
            }

            var bullet = Instantiate(gunItem.gunDataSO.bulletPrefab, gunItem.bulletSpawnTransform.position + characterController.velocity * velocityMod, randomRot).GetComponent<BulletController>();

            if (gunItem.gunDataSO.isConeCast)
            {
                bullet.moveInDirection = true;
            }

            bullet.transform.forward = coneVector;
            bullet.targetPos = targetPos;
            bullet.spawnTransform = gunItem.bulletSpawnTransform;

        }

        currentBulletCount--;
        lastFireTime = Time.time;
        timeBeforeReload = Time.time;
        lastReloadTime = 0;
        crosshairController.AnimateCrosshair();
        if (gunItem.bulletCountParent.childCount > 0)
        {
            Destroy(gunItem.bulletCountParent.GetChild(0).gameObject);
        }

        Recoil();
        Instantiate(fireSFX).PlaySFX();
        //gunItem.shellSpawner.Spawn(gunItem.transform, characterController.velocity);
    }

    IEnumerator ConeCast()
    {
        yield return new WaitForEndOfFrame();

        coneCollider.gameObject.SetActive(true);

        yield return new WaitForEndOfFrame();

        coneCollider.gameObject.SetActive(false);
    }
    void Recoil()
    {
        gunItem.transform.DOLocalMoveX(gunItem.gunDataSO.recoilDistanceStartX, gunItem.gunDataSO.recoilTimeStart).OnComplete(() =>
        {
            gunItem.transform.DOLocalMoveX(gunItem.gunDataSO.recoilDistanceEndX, gunItem.gunDataSO.recoilTimeEnd);
        });

        gunItem.transform.DOLocalMoveZ(gunItem.gunDataSO.recoilDistanceStartY, gunItem.gunDataSO.recoilTimeStart).OnComplete(() =>
        {
            gunItem.transform.DOLocalMoveZ(gunItem.gunDataSO.recoilDistanceEndY, gunItem.gunDataSO.recoilTimeEnd);
        });

        gunItem.transform.DOLocalRotate(gunItem.gunDataSO.recoilAngleStart, gunItem.gunDataSO.recoilTimeStart).OnComplete(() =>
        {
            gunItem.transform.DOLocalRotate(gunItem.gunDataSO.recoilAngleEnd, gunItem.gunDataSO.recoilTimeEnd);
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

            Instantiate(bulletCountPrefab, gunItem.bulletCountParent);

            Instantiate(reloadSFX).PlaySFX();
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
