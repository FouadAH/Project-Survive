using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EntityData : ScriptableObject
{
    public float moveSpeed;
    public float turnSpeed;

    public float velocitySmoothing;

    public float maxHealth;
    public float minAggroRange;
    public float maxAggroRange;
    public float attackRangeMin;
    public float attackRangeMax;


    public LayerMask playerMask;
    public LayerMask obstacleMask;
}
