using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class AvoidBehaviour : BehaviourBase
{
    public float minDetectionDistance = 5f;
    public float minAvoidDistance = 5f;

    public int resolution = 2;
    public int dangerMapResolution = 2;

    public float falloffRate = 0.1f;

    public LayerMask obstacleMask;

    public bool drawGizmos;
    private List<Transform> obstacles = new();

    int instanceID;
    public override void Start()
    {
        base.Start();
        instanceID = transform.GetComponent<Entity>().GetInstanceID();
    }

    private void Update()
    {
        //var colliders = Physics.OverlapSphere(transform.position, minDetectionDistance, obstacleMask);
        //obstacles.Clear();
        //foreach (var collider in colliders)
        //{
        //    if (collider.transform != transform)
        //    {
        //        obstacles.Add(collider.transform);
        //    }
        //}
        DistanceChecked();
    }

    public void DistanceChecked()
    {
        obstacles.Clear();
        foreach (var entity in EntityManager.Instance.entities)
        {
            if (entity != null)
            {
                if (entity.GetInstanceID() == instanceID)
                    continue;

                Vector3 offset = entity.transform.position - transform.position;
                //float distance = Vector3.Distance(entity.transform.position, transform.position);
                float sqrLen = offset.sqrMagnitude;
                // square the distance we compare with
                if (sqrLen <= minDetectionDistance*minDetectionDistance)
                {
                    if (drawGizmos)
                    {
                        Debug.DrawRay(transform.position, offset, Color.magenta);
                    }

                    obstacles.Add(entity.transform);
                }
            }
        }
    }

    public override float[] ConstructDangerMap()
    {
        float[] dangerMap = new float[contextMap.resolution];

        foreach (var obstacle in obstacles)
        {
            float distance = Vector3.Distance(transform.position, obstacle.position);
            Vector3 toVector = obstacle.position - transform.position;
            float sqrLen = toVector.sqrMagnitude;

            if (sqrLen < minDetectionDistance * minDetectionDistance)
            {
                //Vector3 toVector = obstacle.position - transform.position;

                int index = contextMap.MapDirectionToSlotIndex(toVector);
                if (index != -1)
                {
                    float weight = Utility.Remap(distance, 0, minAvoidDistance, 2, 0);
                    weight = Mathf.Clamp(weight, 0, 1);

                    dangerMap[index] = weight;

                    FillMap(dangerMap, dangerMapResolution, index, weight, falloffRate);
                }
            }
        }

        return dangerMap;
    }

    public override float[] ConstructInterestMap()
    {
        float[] interestMap = new float[contextMap.resolution];

        foreach (var obstacle in obstacles)
        {
            float distance = Vector3.Distance(transform.position, obstacle.position);

            if (distance < minDetectionDistance)
            {
                Vector3 awayVector = transform.position - obstacle.position;
                Vector3 normal = new Vector3(-awayVector.y, awayVector.x, awayVector.z).normalized;
                //Debug.DrawRay(transform.position, normal, Color.yellow);
                //Debug.DrawRay(transform.position, -normal, Color.yellow);

                int index = contextMap.MapDirectionToSlotIndex(awayVector);
                if (index != -1)
                {
                    float weight = Utility.Remap(distance, 0, minAvoidDistance, 1, 0);
                    normal = normal * weight;
                    Debug.DrawRay(transform.position, normal, Color.yellow);

                    float dot = Vector3.Dot(awayVector, normal);

                    weight = 1 - Mathf.Abs(dot - 0.65f);
                    weight = Mathf.Clamp(weight, 0, 1);

                    interestMap[index] = weight;

                    for (int i = 1; i <= resolution; i++)
                    {
                        Vector3 newAway = contextMap.GetHeadingAtIndex((index + i) % interestMap.Length);
                        dot = Vector3.Dot(newAway, normal);

                        weight = 1 - Mathf.Abs(dot - 0.65f) - falloffRate * i;
                        weight = Mathf.Clamp(weight, 0, 1);

                        interestMap[(index + i) % interestMap.Length] = Mathf.Clamp(weight, 0, Mathf.Infinity);

                        int remapedIndex = index - i;
                        if (remapedIndex < 0)
                        {
                            remapedIndex = (int)Utility.Remap(index - i, -1f, (interestMap.Length) * -1, interestMap.Length - 1, 0f);
                        }

                        newAway = contextMap.GetHeadingAtIndex(remapedIndex);
                        dot = Vector3.Dot(newAway, -normal);

                        weight = 1 - Mathf.Abs(dot - 0.65f) - falloffRate * i;
                        weight = Mathf.Clamp(weight, 0, 1);

                        interestMap[remapedIndex] = Mathf.Clamp(weight, 0, Mathf.Infinity);
                    }
                }
            }
        }

        return interestMap;
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, minDetectionDistance);
    }
}
