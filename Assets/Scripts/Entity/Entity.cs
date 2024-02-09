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

    [Header("Checkers")]
    public Transform playerChecker;

    [Header("States")]
    public IdleStateData IdleStateData;
    public PatrolStateData PatrolStateData;
    public PlayerDetectedStateData PlayerDetectedStateData;
    public PulledStateData PulledStateData;

    public IdleState idleState;
    public PatrolState patrolState;
    public PlayerDetectedState playerDetectedState;
    public AttackState attackState;
    public PulledState pulledState;

    [Header("Others")]
    public Transform orientation;
    public MeleeAttackController meleeAttackController;
    public EntityProjectileController projectileController;

    [Header("Ragdoll Settings")]
    public RagdollController ragdoll;

    [Header("Canvas Settings")]
    public Canvas canvas;

    public bool lookAtPlayer;
    protected float health;
    protected Vector3 targetPosition;
    protected InputProvider inputProvider;
    protected AgentController agentController;

    protected ContextMap contextMap;
    protected SeekBehaviour seekBehaviour;
    protected AvoidBehaviour avoidBehaviour;

    protected EntitySpawner entitySpawner;
    protected DamageController damagableBase;

    protected bool isDead;

    public FiniteStateMachine stateMachine;
    public CharacterMovement characterMovement { get; private set; }
    public PhysicsParticleSpawner particleSpawner;
    public PhysicsParticleSpawner particleSpawner_currency;

    public Vector2 spawnPosition { get; private set; }
    public bool isVisible;
    public bool canMove = true;

    public virtual void Start()
    {
        canMove = true;
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

        damagableBase = GetComponent<DamageController>();
        damagableBase.Init(entityData.maxHealth);

        stateMachine = new FiniteStateMachine();
        idleState = new IdleState(this, stateMachine, IdleStateData);
        patrolState = new PatrolState(this, stateMachine, PatrolStateData);
        playerDetectedState = new PlayerDetectedState(this, stateMachine, PlayerDetectedStateData);
        attackState = new AttackState(this, stateMachine, PlayerDetectedStateData);
        pulledState = new PulledState(this, stateMachine, PulledStateData);

        stateMachine.Initialize(idleState);
    }

    Vector3 lookPos;
    Quaternion targetRot;

    public virtual void Update()
    {
        if (canMove)
        {
            inputProvider.OnMove(CalculateMovementDirection());
        }
        else
        {
            inputProvider.OnMove(Vector3.zero);
        }

        if (displayText)
        {
            debugText.text = stateMachine.currentState.ToString();
        }

        stateMachine.currentState.LogicUpdate();


        if (lookAtPlayer)
        {
            lookPos = runtimeData.playerPosition - transform.position;

            targetRot = Quaternion.LookRotation(lookPos);
            orientation.rotation = Quaternion.Slerp(orientation.rotation, targetRot, Time.deltaTime * entityData.turnSpeed);
        }
        else
        {
            lookPos = targetPosition - transform.position;

            targetRot = Quaternion.LookRotation(lookPos);
            orientation.rotation = Quaternion.Slerp(orientation.rotation, targetRot, Time.deltaTime * entityData.turnSpeed);
        }
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }
    public virtual void OnDeath(Vector3 damageDirection, float damageForce)
    {
        if (isDead)
            return;

        isDead = true;

        characterMovement.enabled = false;

        orientation.gameObject.SetActive(false);

        ragdoll.transform.rotation = orientation.rotation;
        ragdoll.gameObject.SetActive(true);
        ragdoll.AddForce(damageDirection, damageForce, ForceMode.Impulse);

        canvas.gameObject.SetActive(false);
        damagableBase.enabled = false;
        enabled = false;

        EntityManager.Instance.RemoveEntity(this);
        particleSpawner.Spawn();
        particleSpawner_currency.Spawn();
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
        return DistanceChecked(transform.position, runtimeData.playerPosition, entityData.minAggroRange);
    }
    public bool PlayerWithinRange_Max()
    {
        return DistanceChecked(transform.position, runtimeData.playerPosition, entityData.maxAggroRange);
    }
    public bool PlayerWithinMeleeAttackRange()
    {
        return DistanceChecked(meleeAttackController.transform.position, runtimeData.playerPosition, meleeAttackController.radius);
    }

    public bool PlayerWithinRangedAttackRange_Min()
    {
        return DistanceChecked(projectileController.transform.position, runtimeData.playerPosition, entityData.attackRangeMin);
    }
    public bool PlayerWithinRangedAttackRange_Max()
    {
        return DistanceChecked(projectileController.transform.position, runtimeData.playerPosition, entityData.attackRangeMax);
    }
    public bool DistanceChecked(Vector3 position, Vector3 target, float distance)
    {
        Vector3 offset =  target - position;
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

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerChecker.position, entityData.attackRangeMin);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerChecker.position, entityData.attackRangeMax);
    }

    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }
}