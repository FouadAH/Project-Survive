using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulledState : State
{
    private PulledStateData stateData;
    public PulledState(Entity entity, FiniteStateMachine stateMachine, PulledStateData stateData) : base(entity, stateMachine)
    {
        this.stateData = stateData;
    }
    public override void Enter()
    {
        base.Enter();
        entity.characterMovement.moveSpeed = stateData.pullSpeed;
        //entity.characterMovement.velocitySmoothTime = stateData.pullVelocitySmoothing;
    }

    public override void Exit()
    {
        base.Exit();
        entity.characterMovement.moveSpeed = entity.entityData.moveSpeed;
        //entity.characterMovement.velocitySmoothTime = entity.entityData.velocitySmoothing;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //float distanceToPlayer = Vector2.Distance(entity.transform.position, entity.runtimeData.playerRuntimePosition);
        //if (distanceToPlayer < stateData.maxPullDistance)
        //{
        //    entity.stateMachine.ChangeState(entity.idleState);
        //}
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        //Vector2 directionToPlayer = entity.runtimeData.playerRuntimePosition - (Vector2)entity.transform.position;
        //entity.characterMovement.SetMovementDirection(directionToPlayer);
    }
}
