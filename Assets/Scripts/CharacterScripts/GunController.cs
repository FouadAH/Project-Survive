using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

public class GunController : MonoBehaviour
{
    public GunItem gunItem;
    public PlayerAbilityDataSO playerAbilityDataSO;
    public Transform targetObject;
    public LayerMask obstacleMask;
    public Collider coneCollider;

    [Header("Effect Settings")]
    public GameObject decalPrefab;

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

    private void Start()
    {
        gunItem.InitGun();
    }

    private void Update()
    {
        if (gunItem == null)
            return;

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
        gunItem.InitGun();

        leftHandIK.data.target = gunItem.leftHandTarget;
        rightHandIK.data.target = gunItem.rightHandTarget;
        rigBuilder.Build();
    }

    public void Fire(CharacterController characterController)
    {
        this.characterController = characterController;

        if (!gunItem.CanFire())
        {
            return;
        }

        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 projection = (Camera.main.transform.forward) * 100000f;
        Vector3 direction = projection - cameraPos;
        Vector3 targetPos = direction.normalized * 100000f;

        if (gunItem.gunDataSO.isTripleShot)
        {
            SpawnBullet(targetPos, Quaternion.Euler(0, 0, 0));
            SpawnBullet(targetPos, Quaternion.Euler(0, 10f, 0));
            SpawnBullet(targetPos, Quaternion.Euler(0f, -10f, 0));
        }
        else if (gunItem.gunDataSO.isPentaShot)
        {
            SpawnBullet(targetPos, Quaternion.Euler(0, 0, 0));
            SpawnBullet(targetPos, Quaternion.Euler(0, 10f, 0));
            SpawnBullet(targetPos, Quaternion.Euler(0f, -10f, 0));
            SpawnBullet(targetPos, Quaternion.Euler(0, 0, -10));
            SpawnBullet(targetPos, Quaternion.Euler(0f, 0f, 10));
        }
        else
        {
            SpawnBullet(targetPos, Quaternion.Euler(0, 0, 0));
        }

        gunItem.OnFire();
    }
    void SpawnBullet(Vector3 targetPosition, Quaternion rotation)
    {
        Vector3 forwardVector = rotation * Camera.main.transform.forward;

        var bullet = ObjectPool.instance.Get(gunItem.gunDataSO.bulletPrefab).GetComponent<BulletController>();

        bullet.SetDamage(damageSource.damageValue);
        bullet.OnDamage = () =>
        {
            crosshairController.AnimateCrosshair_HitWeakSpot();
        };

        bullet.transform.position = gunItem.bulletSpawnTransform.position + characterController.velocity * velocityMod;
        bullet.transform.rotation = rotation;

        bullet.moveInDirection = true;

        bullet.transform.forward = forwardVector;
        bullet.targetPos = targetPosition;
        bullet.spawnTransform = gunItem.bulletSpawnTransform;
    }

    void SpawnDecal(Vector3 normal, Vector3 position, Transform parent)
    {
        Quaternion quaternion = Quaternion.LookRotation(-normal, Vector3.up);
        var decal = Instantiate(decalPrefab, position, quaternion);
        //float randomRotation = Random.Range(0, 180f);
        //decal.transform.Rotate(decal.transform.forward, randomRotation);
        decal.transform.parent = parent;
    }

    IEnumerator ConeCast()
    {
        yield return new WaitForEndOfFrame();
        coneCollider.gameObject.SetActive(true);

        yield return new WaitForEndOfFrame();
        coneCollider.gameObject.SetActive(false);
    }
}
