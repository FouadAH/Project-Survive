using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PatrolState : State
{
    private PatrolStateData stateData;
    private Vector2 targetPosition;
    private Vector2 moveDirection;

    public PatrolState(Entity entity, FiniteStateMachine stateMachine, PatrolStateData stateData) : base(entity, stateMachine)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        targetPosition = (Vector3)entity.spawnPosition + Random.insideUnitSphere * stateData.patrolRange;
        moveDirection = targetPosition - (Vector2)entity.transform.position;
    }

    public override void Exit()
    { 
        base.Exit();
        //entity.characterMovement.SetMovementDirection(Vector2.zero);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        float distanceToTarget = Vector2.Distance(entity.transform.position, targetPosition);
        if (entity.IsDetectingWall() || distanceToTarget <= stateData.stoppingDistance)
        {
            LookForValidTarget();
        }
        else if (entity.PlayerWithinRange_Min())
        {
            entity.stateMachine.ChangeState(entity.playerDetectedState);
        }
    }

    void LookForValidTarget()
    {
        targetPosition = (Vector3)entity.spawnPosition + Random.insideUnitSphere * stateData.patrolRange;
        moveDirection = targetPosition - (Vector2)entity.transform.position;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        //entity.characterMovement.SetMovementDirection(moveDirection.normalized);
    }
}
