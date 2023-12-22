using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputProvider))]
public class EnemyController : MonoBehaviour
{
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    public float turnSpeed = 10f;
    public Transform orientation;
    public PlayerRuntimeDataSO playerRuntimeDataSO;

    private Vector3 targetPosition;

    private InputProvider inputProvider;
    private AgentController agentController;

    private void Awake()
    {
        inputProvider = GetComponent<InputProvider>();
        agentController = GetComponent<AgentController>();
    }

    private void Update()
    {
        agentController.SetTarget(playerRuntimeDataSO.playerPosition);
        var path = agentController.GetPath();

        float distance = Vector3.Distance(targetPosition, transform.position);

        targetPosition = path.corners[1];
        Vector3 origin = path.corners[0];

        Vector3 directionToTarget = (targetPosition - origin).normalized;
        directionToTarget = new Vector3(directionToTarget.x, 0, directionToTarget.z);

        inputProvider.OnMove(directionToTarget);

        Vector3 lookPos = targetPosition - transform.position;
        lookPos.y = 0;

        Quaternion targetRot = Quaternion.LookRotation(lookPos);
        orientation.rotation = Quaternion.Slerp(orientation.rotation, targetRot, Time.deltaTime * turnSpeed);

        Debug.Log($"distance: {distance}; direction {directionToTarget}");
    }

    [ContextMenu("Jump")]
    public void Jump()
    {
        inputProvider.OnJump();
    }

    [ContextMenu("Start Hover")]
    public void StartHover()
    {
        inputProvider.OnStartHover();
    }

    [ContextMenu("Stop Hover")]
    public void StopHover()
    {
        inputProvider.OnStopHover();
    }
}
