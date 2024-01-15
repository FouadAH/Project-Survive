using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    protected IdleStateData stateData;

    protected float idleTime;
    protected bool isIdleTimeOver;

    public IdleState(Entity entity, FiniteStateMachine stateMachine, IdleStateData stateData) : base(entity, stateMachine)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        isIdleTimeOver = false;
        idleTime = Random.Range(stateData.idleTimeMin, stateData.idleTimeMax);

        //entity.characterMovement.SetMovementDirection(Vector2.zero);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= startTime + idleTime)
        {
            isIdleTimeOver = true;
            entity.stateMachine.ChangeState(entity.patrolState);
        }
        else if (entity.PlayerWithinRange_Min())
        {
            entity.stateMachine.ChangeState(entity.playerDetectedState);
        }
    }
}
