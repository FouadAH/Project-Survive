using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launchable : MonoBehaviour
{
    public float damageAmount = 10f;

    public float launchSpeed = 10f;
    public float launchDistance = 4f;
    public float launchDuration = 2f;

    public Collider collisionCollider;
    public Collider triggerCollider;

    private Vector3 launchDirection;
    private float currentLaunchTime;

    public bool checkDamageTrigger;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!checkDamageTrigger)
            return;
        
        var damageable = other?.GetComponent<DamagableBase>();
        if (damageable)
        {
            damageable.TakeDamage(damageAmount, Vector2.zero);
            //checkDamageTrigger = false;
        }
    }

    void Launch()
    {
        launchDirection = Camera.main.transform.forward;

        var launchable = rb.GetComponent<Launchable>();
        if (launchable != null)
        {
            launchable.checkDamageTrigger = true;
            StartCoroutine(LaunchRoutine());
        }
        else
        {
            rb.AddForce(launchDirection * launchSpeed, ForceMode.Impulse);
            rb.useGravity = true;
        }
    }

    IEnumerator LaunchRoutine()
    {
        rb.isKinematic = true;
        currentLaunchTime = launchDuration;
        while (currentLaunchTime > 0)
        {
            currentLaunchTime -= Time.deltaTime;
            rb.transform.position = Vector3.Lerp(rb.transform.position, rb.transform.position + launchDirection * launchDistance, 0.1f);
            yield return new WaitForFixedUpdate();
        }

        rb.isKinematic = false;
        rb.useGravity = true;
        checkDamageTrigger = false;
    }

}
