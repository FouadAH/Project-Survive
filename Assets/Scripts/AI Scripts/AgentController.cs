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

    private CharacterController characterController;
    private float elapsed = 0.0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<CharacterController>();

        agent.updatePosition = false;
        agent.updateRotation = false;
        elapsed = 0.0f;
    }

    void Update()
    {
        agent.nextPosition = transform.position;
        agent.velocity = characterController.velocity;

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
