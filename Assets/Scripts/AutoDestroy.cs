using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float destroyTime = 1f;
    private void Start()
    {
        StartCoroutine(AutoDestroyRoutine());
    }

    IEnumerator AutoDestroyRoutine()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
