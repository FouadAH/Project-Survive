using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using static UnityEngine.EventSystems.EventTrigger;

public class Entity : EntityBase
{
    public EntityData entityData;

    public PlayerRuntimeDataSO runtimeData;

    [Header("Debug Text")]
    public bool drawGizmos;
    public bool displayText;

    public TMPro.TMP_Text debugText;

    //[Header("Checkers")]
    //public Transform wallChecker;

    [Header("States")]
    public IdleStateData IdleStateData;
    public PatrolStateData PatrolStateData;
    public PlayerDetectedStateData PlayerDetectedStateData;
    public PulledStateData PulledStateData;

    public IdleState idleState;
    public PatrolState patrolState;
    public PlayerDetectedState playerDetectedState;
    public PulledState pulledState;

    [Header("Others")]
    public Transform orientation;
    public MeleeAttackController meleeAttackController;
    public RagdollController ragdoll;
    public Canvas canvas;
    protected float health;
    protected Vector3 targetPosition;
    protected InputProvider inputProvider;
    protected AgentController agentController;

    protected ContextMap contextMap;
    protected SeekBehaviour seekBehaviour;
    protected AvoidBehaviour avoidBehaviour;

    protected EntitySpawner entitySpawner;
    //protected ProjectileController projectileController;

    public FiniteStateMachine stateMachine;
    public CharacterMovement characterMovement { get; private set; }

    public Vector2 spawnPosition { get; private set; }

    public virtual void Start()
    {
        health = entityData.maxHealth;
        spawnPosition = transform.position;

        contextMap = GetComponent<ContextMap>();
        seekBehaviour = GetComponent<SeekBehaviour>();
        seekBehaviour.maxDetectDistance = entityData.minAggroRange;

        avoidBehaviour = GetComponent<AvoidBehaviour>();

        inputProvider = GetComponent<InputProvider>();
        agentController = GetComponent<AgentController>();

        characterMovement = GetComponent<CharacterMovement>();
        characterMovement.moveSpeed = entityData.moveSpeed;
        //characterMovement.velocitySmoothTime = entityData.velocitySmoothing;

        //projectileController = GetComponent<ProjectileController>();

        stateMachine = new FiniteStateMachine();
        idleState = new IdleState(this, stateMachine, IdleStateData);
        patrolState = new PatrolState(this, stateMachine, PatrolStateData);
        playerDetectedState = new PlayerDetectedState(this, stateMachine, PlayerDetectedStateData);
        pulledState = new PulledState(this, stateMachine, PulledStateData);

        stateMachine.Initialize(idleState);
    }

    Vector3 lookPos;
    Quaternion targetRot;

    public virtual void Update()
    {
        inputProvider.OnMove(CalculateMovementDirection());

        if (displayText)
        {
            debugText.text = stateMachine.currentState.ToString();
        }

        stateMachine.currentState.LogicUpdate();

        lookPos = targetPosition - transform.position;

        targetRot = Quaternion.LookRotation(lookPos);
        orientation.rotation = Quaternion.Slerp(orientation.rotation, targetRot, Time.deltaTime * entityData.turnSpeed);
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }
    public void OnDeath(Vector3 damageDirection, float damageForce)
    {
        characterMovement.characterController.enabled = false;
        orientation.gameObject.SetActive(false);
        ragdoll.gameObject.SetActive(true);
        ragdoll.AddForce(damageDirection, damageForce, ForceMode.Impulse);
        canvas.gameObject.SetActive(false);
        this.enabled = false;
    }
    public Vector3 CalculateMovementDirection()
    {
        return contextMap.CalculateMovementDirection().normalized;
    }

    public void SetMovementVector(Vector3 movementVector)
    {
        inputProvider.OnMove(movementVector.normalized);
    }

    public void MoveToTarget(Vector3 targetPosition)
    {
        agentController.SetTarget(targetPosition);
        var path = agentController.GetPath();
        if (path.corners.Length > 1)
        {
            this.targetPosition = path.corners[1];
            seekBehaviour.hasTarget = true;
            seekBehaviour.target = path.corners[1];
        }
    }

    public void SetHasTarget(bool hasTarget)
    {
        seekBehaviour.hasTarget = hasTarget;
    }

    public bool PlayerWithinRange_Min()
    {
        return DistanceChecked(runtimeData.playerPosition, entityData.minAggroRange);
    }
    public bool PlayerWithinRange_Max()
    {
        return DistanceChecked(runtimeData.playerPosition, entityData.maxAggroRange);
    }

    public bool DistanceChecked(Vector3 target, float distance)
    {
        Vector3 offset =  target - transform.position;
        float sqrLen = offset.sqrMagnitude;
        return sqrLen < distance * distance;
    }

    public bool IsDetectingWall()
    {
        return false;
    }

    public void SetSpawner(EntitySpawner entitySpawner)
    {
        this.entitySpawner = entitySpawner;
    }

    public void FireProjectile()
    {
        //projectileController.FireProjectile(characterMovement.orientation.rotation);
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, entityData.minAggroRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, entityData.maxAggroRange);
    }
}