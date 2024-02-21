using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaggerState : State
{
    public StaggerState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        entity.canMove = false;
        if (entity.staggeredPS != null)
        {
            entity.staggeredPS.Play();
        }
        //entity.lookAtPlayer = false;
    }

    public override void Exit()
    {
        base.Exit();
        entity.canMove = true;
        if (entity.staggeredPS != null)
        {
            entity.staggeredPS.Stop();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!entity.isStaggered)
        {
            stateMachine.ChangeState(entity.idleState);
        }
    }
}
