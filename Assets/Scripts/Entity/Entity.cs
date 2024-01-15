using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class Entity : MonoBehaviour
{
    public EntityData entityData;

    public PlayerRuntimeDataSO runtimeData;

    [Header("Debug Text")]
    public TMPro.TMP_Text debugText;

    [Header("Checkers")]
    public Transform wallChecker;

    [Header("States")]
    public IdleStateData IdleStateData;
    public PatrolStateData PatrolStateData;
    public PlayerDetectedStateData PlayerDetectedStateData;
    public PulledStateData PulledStateData;

    public IdleState idleState;
    public PatrolState patrolState;
    public PlayerDetectedState playerDetectedState;
    public PulledState pulledState;

    private float health;

    private EntitySpawner entitySpawner;
    //private ProjectileController projectileController;

    public FiniteStateMachine stateMachine;
    public CharacterMovement characterMovement { get; private set; }

    public Vector2 spawnPosition { get; private set; }

    public virtual void Start()
    {
        health = entityData.maxHealth;
        spawnPosition = transform.position;

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

    public virtual void Update()
    {
        debugText.text = stateMachine.currentState.ToString();

        stateMachine.currentState.LogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public void TakeDamage(float damageAmount, Vector2 damageDirection)
    {
        health -= damageAmount;

        Vector2 directionToPlayer = (Vector2)transform.position - damageDirection;
        //characterMovement.KnockBack(directionToPlayer);

        if (health <= 0)
        {
            if (entitySpawner != null)
            {
                entitySpawner.Despawn(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public bool PlayerWithinRange_Min()
    {
        bool detectedObstacle = Physics2D.Linecast(transform.position, runtimeData.playerPosition, entityData.obstacleMask);
        bool playerInRange = Physics2D.OverlapCircle(transform.position, entityData.minAggroRange, entityData.playerMask);
        return playerInRange && !detectedObstacle;
    }
    public bool PlayerWithinRange_Max()
    {
        bool detectedObstacle = Physics2D.Linecast(transform.position, runtimeData.playerPosition, entityData.obstacleMask);
        bool playerInRange = Physics2D.OverlapCircle(transform.position, entityData.maxAggroRange, entityData.playerMask);
        return playerInRange && !detectedObstacle;
    }

    public bool IsDetectingWall()
    {
        return Physics2D.Linecast(transform.position, wallChecker.position, entityData.obstacleMask);
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, entityData.minAggroRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, entityData.maxAggroRange);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, wallChecker.position);
    }
}