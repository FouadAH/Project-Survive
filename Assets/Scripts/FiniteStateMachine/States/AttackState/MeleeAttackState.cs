using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class MeleeAttackState : AttackState
{
    public MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, PlayerDetectedStateData stateData) : base(entity, stateMachine, stateData)
    {
        this.stateData = stateData;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (RangeCheck())
        {
            entity.meleeAttackController.ProcessAttack();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        entity.SetHasTarget(false);
    }

    public override bool RangeCheck()
    {
        return entity.PlayerWithinMeleeAttackRange();
    }
}
