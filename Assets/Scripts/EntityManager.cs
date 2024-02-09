using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance;
    public List<EntityBase> entities = new();
    public List<Vector2> positions = new();

    public List<Transform> targets = new();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            entities = FindObjectsByType<EntityBase>(FindObjectsSortMode.InstanceID).ToList();
            foreach(EntityBase entity in entities)
            {
                positions.Add(new Vector2(entity.transform.position.x, entity.transform.position.z));
            }
        }
    }

    private void Update()
    {
        int i = 0;
        foreach(EntityBase entity in entities)
        {
            positions[i] = new Vector2(entity.transform.position.x, entity.transform.position.z);
            i++;
        }
    }

    public void AddEntity(EntityBase entity)
    {
        entities.Add(entity);
        positions.Add(new Vector2(entity.transform.position.x, entity.transform.position.z));
    }

    public void RemoveEntity(EntityBase entity)
    {
        entities.Remove(entity);
        positions.Remove(new Vector2(entity.transform.position.x, entity.transform.position.z));
    }

    public void AddTarget(Transform entity)
    {
        targets.Add(entity);
    }
}
