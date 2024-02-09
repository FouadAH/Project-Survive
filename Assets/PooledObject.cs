using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
