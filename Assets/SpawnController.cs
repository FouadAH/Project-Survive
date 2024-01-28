using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public int spawnCurrency = 100;
    public int maxSpawnCount = 10;
    private int spawnCount = 0;

    public Vector2 spawnOffsetX;
    public float spawnWaitTime = 3f;
    public EntitySpawnDataList spawnDataList = new EntitySpawnDataList();
    private Queue<Entity> spawnQueue = new Queue<Entity>();
    System.Random rand = new System.Random();
    private void Start()
    {
        ConstructSpawnQueue();
    }

    [ContextMenu("Construct Queue")]
    public void ConstructSpawnQueue()
    {
        StartCoroutine(ConstructSpawnQueueRoutine());
        StartCoroutine(SpawnRoutine());
    }
    IEnumerator ConstructSpawnQueueRoutine()
    {
        spawnQueue.Clear();
        rand = new System.Random();
        while (spawnCount < maxSpawnCount)
        {
            float randomWeight = rand.Next(0, 100);
            foreach (EntitySpawnData spawnData in spawnDataList.spawnData)
            {
                if (spawnCurrency >= spawnData.spawnCost && randomWeight >= 100 - 100 * spawnData.spawnWeight && spawnCount < maxSpawnCount)
                {
                    spawnQueue.Enqueue(spawnData.entity);
                    spawnCurrency -= spawnData.spawnCost;
                    spawnCount++;
                    continue;
                }
            }

            if (spawnCurrency <= 0)
            {
                break;
            }

            yield return null;
        }
    }
    IEnumerator SpawnRoutine()
    {
        while(spawnQueue.Count > 0)
        {
            var entity = spawnQueue.Dequeue();
            float randomOffsetX = UnityEngine.Random.Range(spawnOffsetX.x, spawnOffsetX.y);
            float randomOffsetZ = UnityEngine.Random.Range(spawnOffsetX.x, spawnOffsetX.y);

            Vector3 spawnPosition = new Vector3(transform.position.x + randomOffsetX, transform.position.y, transform.position.z + randomOffsetZ);
            var entitySpawned = Instantiate(entity, spawnPosition, Quaternion.identity);
            EntityManager.Instance.AddEntity(entitySpawned);
            yield return new WaitForSeconds(spawnWaitTime);
        }
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
