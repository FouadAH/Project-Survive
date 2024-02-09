using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class EntityProjectileController : MonoBehaviour
{
    public ProjectileBase projectilePrefab;
    public int projectileBurstCount = 1;
    public int targetMotionStrength = 1;

    public Vector2 randomOffsetX;
    public Vector2 randomOffsetY;
    public Vector2 randomOffsetZ;

    public float attackCooldown;
    private float attackCooldownCurrent;
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        if (attackCooldownCurrent > 0)
        {
            attackCooldownCurrent -= Time.deltaTime;
        }
        else
        {
            attackCooldownCurrent = 0;
        }
    }
    Vector3 pos;
    public void LaunchProjectile(Vector3 targetPosition, Vector3 targetMotion)
    {
        if (attackCooldownCurrent != 0)
            return;

        attackCooldownCurrent = attackCooldown;

        for (int i = 0; i < projectileBurstCount; i++)
        {
            var projectile = ObjectPool.instance.Get(projectilePrefab) as ProjectileBase;

            projectile.transform.position = transform.position;
            projectile.transform.rotation = Quaternion.identity;

            float offsetX = Random.Range(randomOffsetX.x, randomOffsetX.y);
            float offsetY = Random.Range(randomOffsetY.x, randomOffsetY.y);
            float offsetZ = Random.Range(randomOffsetZ.x, randomOffsetZ.y);

            targetPosition.x += offsetX;
            targetPosition.y += offsetY;
            targetPosition.z += offsetZ;
            pos = targetPosition + targetMotion * targetMotionStrength;
            projectile.Launch(targetPosition + targetMotion * targetMotionStrength);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pos, 1);
    }
}
