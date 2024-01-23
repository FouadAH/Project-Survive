using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaunchController : MonoBehaviour
{
    public Transform launchPivot;
    public Rigidbody currentLaunchable;
    public LayerMask launchableMask;

    public float launchSpeed = 10f;
    public float launchDistance = 4f;

    public float launchDuration = 2f;

    private Vector3 launchDirection;
    private float currentLaunchTime;

    public float suckAccelaration = 0.1f;
    public float maxDetectionRange = 100f;

    private bool isLaunching;
    Coroutine suckRoutine;

    public void OnLaunch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (isLaunching)
                return;

            if (currentLaunchable != null)
            {
                Launch();
            }
            else
            {
                Suck();
            }
        }
    }

    void Launch()
    {
        StopCoroutine(suckRoutine);
        //currentLaunchable.isKinematic = true;

        launchDirection = Camera.main.transform.forward;

        var launchable = currentLaunchable.GetComponent<Launchable>();
        if (launchable != null)
        {
            launchable.checkDamageTrigger = true;
            StartCoroutine(LaunchRoutine());
        }
        else
        {
            currentLaunchable.AddForce(launchDirection * launchSpeed, ForceMode.Impulse);
            currentLaunchable.useGravity = true;
            currentLaunchable = null;
        }
    }

    IEnumerator LaunchRoutine()
    {
        isLaunching = true;
        currentLaunchable.isKinematic = true;
        currentLaunchTime = launchDuration;
        while (currentLaunchTime > 0)
        {
            currentLaunchTime -= Time.deltaTime;
            currentLaunchable.transform.position = Vector3.Lerp(currentLaunchable.transform.position, currentLaunchable.transform.position + launchDirection * launchDistance, 0.1f);
            yield return new WaitForFixedUpdate();
        }

        currentLaunchable.isKinematic = false;
        currentLaunchable.useGravity = true;
        currentLaunchable.GetComponent<Launchable>().checkDamageTrigger = false;
        currentLaunchable = null;

        isLaunching = false;
    }

    void Suck()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDetectionRange, launchableMask);

        if (hit.collider != null)
        {
            var launchable = hit.collider.GetComponent<Rigidbody>();
            if (launchable != null)
            {
                if (launchable.isKinematic == false)
                {
                    currentLaunchable = launchable;
                    currentLaunchable.useGravity = false;
                    suckRoutine = StartCoroutine(SuckRoutine());
                }
            }
        }
    }

    IEnumerator SuckRoutine()
    {
        while (true)
        {
            if (currentLaunchable != null)
            {
                bool isNear = Vector3.Distance(currentLaunchable.transform.position, launchPivot.position) <= 1f;
                float acc = (isNear) ? 0.3f : suckAccelaration;

                currentLaunchable.transform.position = Vector3.Lerp(currentLaunchable.transform.position, launchPivot.position, acc);
                yield return new WaitForFixedUpdate();
            }
        }
    }

}
