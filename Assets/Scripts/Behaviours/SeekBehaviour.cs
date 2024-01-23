using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SeekBehaviour : BehaviourBase
{
    public float maxDetectDistance = 5f;
    public float maxSeekDistance = 5f;

    public int resolution = 2;
    public float falloffRate = 0.1f;
    public bool isSingleTarget;
    public LayerMask targetMask;
    public bool drawGizmos;

    public bool hasTarget;
    private List<Vector3> targets = new();

    public Vector3 target;

    private void Update()
    {
        if (isSingleTarget)
            return;

        //var colliders = Physics.OverlapSphere(transform.position, maxDetectDistance, targetMask);
        //targets.Clear();
        //foreach (var collider in colliders)
        //{
        //    targets.Add(collider.transform.position);
        //}
    }

    public override float[] ConstructInterestMap()
    {
        var interestMap = new float[contextMap.resolution];
        
        if (!hasTarget)
            return interestMap;

        float distance = Vector3.Distance(transform.position, target);

        if (distance <= maxDetectDistance)
        {
            Vector3 direction = target - transform.position;
            int index = contextMap.MapDirectionToSlotIndex(direction);
            if (index != -1)
            {
                float weight = Utility.Remap(distance, 0, maxDetectDistance, 0, 1);
                weight = Mathf.Clamp(weight, 0, 1);
                interestMap[index] = weight;

                FillMap(interestMap, resolution, index, weight, falloffRate);
            }
        }
        return interestMap;
    }

    public void SetTargetPosition(Vector3 target, bool hasTarget)
    {
        this.hasTarget = hasTarget;
        this.target = target;
    }

    public void SetTargets(List<Vector3> targets)
    {
        this.targets = targets;
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDetectDistance);
    }
}
