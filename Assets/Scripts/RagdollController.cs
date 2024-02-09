using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : DamagableBase
{
    public Rigidbody body;

    private void OnEnable()
    {
        if (gameObject.GetComponent<AutoDestroy>() == null)
        {
            gameObject.AddComponent<AutoDestroy>().destroyTime = 3f;
        }

        transform.parent.gameObject.AddComponent<AutoDestroy>().destroyTime = 3f;
    }

    public override void TakeDamage(float damageValue, Vector3 normal, float force = 1)
    {
        AddForce(normal, force, ForceMode.Impulse);
    }

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
