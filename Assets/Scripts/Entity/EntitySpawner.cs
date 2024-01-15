using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    public Entity entityPrefab;
    public Transform spawnParent;

    [Header("Spawn Settings")]
    public int maxSpawnCount;
    public float spawnRange;
    public float spawnCooldown;
    public bool spawnOnStart;

    private float currentSpawnTime;
    private int spawnCount;

    private void Start()
    {
        if (spawnOnStart)
        {
            currentSpawnTime = spawnCooldown;
        }
    }

    private void Update()
    {
        if (spawnCount >= maxSpawnCount)
            return;

        currentSpawnTime += Time.deltaTime;
        if(currentSpawnTime >= spawnCooldown)
        {
            Spawn();
        }
    }

    public void Spawn() 
    { 
        Vector2 spawnPosition = transform.position + Random.insideUnitSphere * spawnRange;

        var enity = Instantiate(entityPrefab, spawnPosition, Quaternion.identity, spawnParent);
        enity.SetSpawner(this);

        currentSpawnTime = 0;
        spawnCount++;
    }

    public void Despawn(Entity entity)
    {
        Destroy(entity.gameObject);
        spawnCount--;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
}
