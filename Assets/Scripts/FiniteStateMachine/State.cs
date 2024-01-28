using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public float startTime { get; protected set; }
    protected FiniteStateMachine stateMachine;
    protected Entity entity;

    public State(Entity entity, FiniteStateMachine stateMachine)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        startTime = Time.time;
        DoChecks();
    }

    public virtual void Exit()
    {
    }

    public virtual void LogicUpdate()
    {
    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks()
    {
    }
}