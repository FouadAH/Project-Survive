using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerAttackState : AttackState
{
    FlamethrowerSpider flamethrowerSpider;
    public float attackCooldown = 5f;
    private float attackCooldownCurrent;

    public FlamethrowerAttackState(Entity entity, FiniteStateMachine stateMachine, PlayerDetectedStateData stateData) : base(entity, stateMachine, stateData)
    {
        flamethrowerSpider = entity as FlamethrowerSpider;
    }

    public override void Enter()
    {
        base.Enter();
        attackCooldown = flamethrowerSpider.attackTime;
        flamethrowerSpider.flamethrowerController.ToggleActivateDamageTrigger(true);
        entity.lookAtPlayer = true;
        entity.canMove = false;
    }

    public override void Exit()
    {
        base.Exit();
        flamethrowerSpider.flamethrowerController.ToggleActivateDamageTrigger(false);
        entity.lookAtPlayer = false;
        entity.canMove = true;
    }

    public override void LogicUpdate()
    {
        attackCooldown -= Time.deltaTime;
        if(attackCooldown < 0)
        {
            Debug.Log("end attack");
            stateMachine.ChangeState(entity.playerDetectedState);
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
