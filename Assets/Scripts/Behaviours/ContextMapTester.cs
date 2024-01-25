using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextMapTester : MonoBehaviour
{
    private ContextMap contextMap;
    private SeekBehaviour seekBehaviour;

    void Start()
    {
        contextMap = GetComponent<ContextMap>();
        seekBehaviour = GetComponent<SeekBehaviour>();
    }

    void Update()
    {
        contextMap.CalculateMovementDirection();
        if (EntityManager.Instance.targets.Count > 0) 
        {
            seekBehaviour.SetTargetPosition(EntityManager.Instance.targets[0].position, true);
        }
    }
}
