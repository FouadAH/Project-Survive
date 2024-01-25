using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BulletController : MonoBehaviour
{
    public float speed;
    public Vector3 targetPos;
    public bool moveInDirection;
    public ParticleSystem bulletDestroyEffect;
    public LayerMask obstacleMask;

    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private bool shouldMove;
    public Transform spawnTransform;
    Vector3 initialSpawn;
    Vector3 velocityDirection;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        initialSpawn = transform.position;
        shouldMove = false;
        StartCoroutine(MovementLock());
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

    IEnumerator MovementLock()
    {
        yield return new WaitForEndOfFrame();
        meshRenderer.enabled = true;
        velocityDirection = initialSpawn - spawnTransform.position;
        transform.position += velocityDirection;
        shouldMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsInLayerMask(obstacleMask, other.gameObject.layer))
        {
            var bullet = Instantiate(bulletDestroyEffect, transform.position, Quaternion.identity);
            bullet.Play();

            Destroy(gameObject);
        }
    }

    bool IsInLayerMask(LayerMask layerMask, int layer)
    {
        return layerMask == (layerMask | (1 << layer));
    }
}
