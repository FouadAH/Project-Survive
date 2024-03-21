using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BulletController : PooledObject
{
    public float speed;
    public Vector3 targetPos;
    public bool moveInDirection;
    public ParticleSystem bulletDestroyEffect;
    public LayerMask obstacleMask;

    public Transform spawnTransform;
    public PlayerAbilityDataSO playerAbilityDataSO;

    public Action OnDamage;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private TrailRenderer trailRenderer;

    private DamageSource damageSource;

    private Vector3 initialSpawn;
    private Vector3 velocityDirection;

    private bool hitLock;
    private bool shouldMove;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
        damageSource = GetComponent<DamageSource>();
    }

    private void OnEnable()
    {
        StartCoroutine(HitLock());
        initialSpawn = transform.position;
        rb.position = initialSpawn;

        shouldMove = true;
        StartCoroutine(MovementLock());
    }

    private void OnDisable()
    {
        shouldMove = false;
        trailRenderer.enabled = false;
        meshRenderer.enabled = false;
    }

    void FixedUpdate()
    {
        if (moveInDirection)
        {
            rb.MovePosition(transform.position + speed * Time.deltaTime * transform.forward);
            return;
        }

        if (shouldMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
    }

    public void SetDamage(float damageValue)
    {
        damageSource.damageValue = damageValue;
    }

    private IEnumerator HitLock()
    {
        yield return new WaitForFixedUpdate();
        hitLock = false;
    }

    private IEnumerator MovementLock()
    {
        yield return new WaitForEndOfFrame();

        meshRenderer.enabled = true;
        //shouldMove = true;
        trailRenderer.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hitLock)
            return;

        if (IsInLayerMask(obstacleMask, other.gameObject.layer))
        {
            var bullet = Instantiate(bulletDestroyEffect, transform.position, Quaternion.identity);
            bullet.Play();

            Vector3 normal = (transform.position - other.transform.position).normalized;
            //SpawnDecal(raycastHit.normal, raycastHit.point, raycastHit.transform);

            if (other.TryGetComponent<WeakSpot>(out var weakSpot))
            {
                weakSpot.DamageController.TakeDamage(damageSource.damageValue * playerAbilityDataSO.weakSpotdamageMod, -normal, 100);
                OnDamage?.Invoke();
            }
            else if (other.TryGetComponent<HitBox>(out var hitbox))
            {
                hitbox.DamageController.TakeDamage(damageSource.damageValue, -normal, 100);
                OnDamage?.Invoke();
            }

            ReturnToPool();
        }
    }

    bool IsInLayerMask(LayerMask layerMask, int layer)
    {
        return layerMask == (layerMask | (1 << layer));
    }
}
