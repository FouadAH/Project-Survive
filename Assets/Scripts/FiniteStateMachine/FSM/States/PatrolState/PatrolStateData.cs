using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="States/Patrol State Data")]
public class PatrolStateData : ScriptableObject
{
    public float patrolRange;
    public float stoppingDistance;
}
