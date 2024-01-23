using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BulletController : MonoBehaviour
{
    public float speed;
    public Vector3 targetPos;

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
        //meshRenderer.enabled = false;
        shouldMove = false;
        StartCoroutine(MovementLock());
    }

    void Update()
    {
        if (shouldMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
        else
        {
            //transform.position = spawnTransform.position;
        }
    }

    IEnumerator MovementLock()
    {
        //transform.position = spawnTransform.position;

        yield return new WaitForEndOfFrame();
        meshRenderer.enabled = true;

        //transform.position = spawnTransform.position;
        velocityDirection = initialSpawn - spawnTransform.position;
        transform.position += velocityDirection;
       shouldMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsInLayerMask(obstacleMask, other.gameObject.layer))
        {
            Destroy(gameObject);
        }
    }

    bool IsInLayerMask(LayerMask layerMask, int layer)
    {
        return layerMask == (layerMask | (1 << layer));
    }
}
