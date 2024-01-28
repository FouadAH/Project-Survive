using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance;
    public List<EntityBase> entities = new();
    public List<Transform> targets = new();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            entities = FindObjectsByType<EntityBase>(FindObjectsSortMode.InstanceID).ToList();
        }
    }

    public void AddEntity(EntityBase entity)
    {
        entities.Add(entity);
    }

    public void RemoveEntity(EntityBase entity)
    {
        entities.Remove(entity);
    }

    public void AddTarget(Transform entity)
    {
        targets.Add(entity);
    }
}
