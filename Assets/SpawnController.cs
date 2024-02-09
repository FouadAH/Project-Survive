using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public Vector2 spawnOffsetX;
    public void Spawn(Entity entity)
    {
        float randomOffsetX = UnityEngine.Random.Range(spawnOffsetX.x, spawnOffsetX.y);
        float randomOffsetZ = UnityEngine.Random.Range(spawnOffsetX.x, spawnOffsetX.y);

        Vector3 spawnPosition = new Vector3(transform.position.x + randomOffsetX, transform.position.y + 2.6f, transform.position.z + randomOffsetZ);
        var entitySpawned = Instantiate(entity, spawnPosition, Quaternion.identity);
        EntityManager.Instance.AddEntity(entitySpawned);
    }
}

[Serializable]
public class EntitySpawnDataList
{
    public List<EntitySpawnData> spawnData;
}

[Serializable]
public class EntitySpawnData
{
    public Entity entity;
    public int spawnCost;

    [Range(0f, 1f)]
    public float spawnWeight;
}
