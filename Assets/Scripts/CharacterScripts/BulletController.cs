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

    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private TrailRenderer trailRenderer;

    private Vector3 initialSpawn;
    private Vector3 velocityDirection;

    private bool hitLock;
    private bool shouldMove;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine(HitLock());
        initialSpawn = transform.position;
        rb.position = initialSpawn;

        shouldMove = false;
        StartCoroutine(MovementLock());
    }

    private void OnDisable()
    {
        shouldMove = false;
        trailRenderer.enabled = false;
    }

    void Update()
    {
        if (moveInDirection)
        {
            rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
            return;
        }

        if (shouldMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
    }

    IEnumerator HitLock()
    {
        yield return new WaitForFixedUpdate();
        hitLock = false;
    }

    IEnumerator MovementLock()
    {
        yield return new WaitForEndOfFrame();
        meshRenderer.enabled = true;
        //velocityDirection = initialSpawn - spawnTransform.position;
        //transform.position += velocityDirection;
        shouldMove = true;
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

            ReturnToPool();
        }
    }

    bool IsInLayerMask(LayerMask layerMask, int layer)
    {
        return layerMask == (layerMask | (1 << layer));
    }
}
