using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaunchController : MonoBehaviour
{
    public Transform launchPivot;
    public Launchable currentLaunchable;
    public LayerMask launchableMask;

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
                currentLaunchable.Launch();

                StopCoroutine(suckRoutine);
                currentLaunchable = null;
            }
            else
            {
                Suck();
            }
        }
    }


    void Suck()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDetectionRange, launchableMask);

        if (hit.collider != null)
        {
            var launchable = hit.collider.GetComponent<Launchable>();
            if (launchable != null)
            {
                currentLaunchable = launchable;
                currentLaunchable.SetGravity(false);
                suckRoutine = StartCoroutine(SuckRoutine());
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
