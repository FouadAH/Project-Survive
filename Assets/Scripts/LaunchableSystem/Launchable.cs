using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launchable : MonoBehaviour
{
    [Header("Daamge Settings")]
    public float damageAmount = 10f;
    public LayerMask damageableMask;
    public bool checkDamageTrigger;
    public bool isPiercing;

    [Header("Launch Settings")]
    public float launchSpeed = 10f;
    public float launchDistance = 4f;
    public float launchDuration = 2f;
    public LayerMask launchableNoCollision;

    private Vector3 launchDirection;
    private float currentLaunchTime;

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
        if (damageable && Utility.IsInLayerMask(damageableMask, damageable.gameObject.layer))
        {
            damageable.TakeDamage(damageAmount, Vector2.zero);

            if (!isPiercing)
            {
                StopAllCoroutines();
                StopLaunch();
            }
        }
    }

    public void Launch()
    {
        launchDirection = Camera.main.transform.forward;

        checkDamageTrigger = true;
        rb.isKinematic = false;
        rb.useGravity = true;

        if (!isPiercing)
        {
            rb.AddForce(launchDirection * launchSpeed, ForceMode.Impulse);
        }
        else
        {
            StartCoroutine(LaunchRoutine());
            gameObject.layer = 8;
        }
    }

    void StopLaunch()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        checkDamageTrigger = false;
    }

    IEnumerator LaunchRoutine()
    {
        currentLaunchTime = launchDuration;
        while (currentLaunchTime > 0)
        {
            currentLaunchTime -= Time.deltaTime;
            rb.transform.position = Vector3.Lerp(rb.transform.position, rb.transform.position + launchDirection * launchDistance, 0.1f);
            yield return new WaitForFixedUpdate();
        }

        StopLaunch();
    }

    public void SetGravity(bool state)
    {
        rb.useGravity = state;
    }

}
