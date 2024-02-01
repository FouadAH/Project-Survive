using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    public float updateRate = 1f;
    private NavMeshAgent agent;
    private Vector3 targetPosition;

    private Rigidbody characterController;
    private float elapsed = 0.0f;
    private Vector3 lastPosition = Vector3.zero;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<Rigidbody>();

        agent.updatePosition = false;
        agent.updateRotation = false;
        elapsed = 0.0f;
    }

    void Update()
    {
        agent.nextPosition = transform.position;
        agent.velocity = transform.position - lastPosition;

        lastPosition = transform.position;

        elapsed += Time.deltaTime;
        if (elapsed > 1f/updateRate)
        {
            elapsed = 0.0f;
            agent.SetDestination(targetPosition);
        }
    }

    public void SetTarget(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    public NavMeshPath GetPath()
    {
        return agent.path;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        for (int i = 0; i < agent.path.corners.Length; i++)
        {
            if(i == 1)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }

            Gizmos.DrawWireSphere(agent.path.corners[i], 1);
        }
    }
}
