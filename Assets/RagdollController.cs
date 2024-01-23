using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public Rigidbody body;

    public void AddForce(Vector3 forceDirection, float force, ForceMode forceMode)
    {
        StartCoroutine(AddForceRoutine(forceDirection, force, forceMode));
    }

    IEnumerator AddForceRoutine(Vector3 forceDirection, float force, ForceMode forceMode)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"add force {forceDirection}, {force}");

        Vector3 direction = Vector3.up * force;
        body.AddForce(direction, forceMode);
    }
}
