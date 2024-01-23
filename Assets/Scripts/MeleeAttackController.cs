using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackController : MonoBehaviour
{
    [Header("Collider Settings")]
    public float radius;
    public LayerMask mask;

    [Header("Damage Settings")]
    public float damage;
    public float attackCooldown;
    private float attackCooldownCurrent;
    private Color stateColor = Color.white;
    private void Update()
    {
        if(attackCooldownCurrent > 0) 
        {
            attackCooldownCurrent -= Time.deltaTime;
            stateColor = Color.yellow;
        }
        else
        {
            attackCooldownCurrent = 0;
        }
    }
    public void ProcessAttack()
    {
        if (attackCooldownCurrent != 0)
            return;

        stateColor = Color.red;

        attackCooldownCurrent = attackCooldown;

        var collider = Physics.OverlapSphere(transform.position, radius, mask);
        if(collider.Length > 0)
        {
            collider[0].gameObject.GetComponent<DamagableBase>()?.TakeDamage(damage, Vector2.zero);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = stateColor;
        Gizmos.DrawWireSphere(transform.position, radius);   
    }
}
