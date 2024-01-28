using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder;

public class ProjectileController : MonoBehaviour
{
    public float linePoints;
    public float transformsBetweenPoints;

    [Range(0f, 1f)]
    public float timeBetweenPoints;
    public float throwStrength;
    public ProjectileBase projectile;

    private Vector3 releasePoint;
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    
    private void Update()
    {
        if (Mouse.current.rightButton.isPressed)
        {
            DisplayProjectile();
        }
    }

    public void DisplayProjectile()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = Mathf.CeilToInt(linePoints / transformsBetweenPoints) + 1;

        Vector3 startPos = transform.position;
        Vector3 startVelocity = throwStrength * Camera.main.transform.forward;

        int i = 0;
        lineRenderer.SetPosition(0, startPos);

        for (float time = 0; time < linePoints; time += timeBetweenPoints)
        {
            i++;

            Vector3 point = startPos + time * startVelocity;
            point.y = startPos.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            lineRenderer.SetPosition(i, point);
        }
    }
}
