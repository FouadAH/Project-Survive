using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CircleLayout : MonoBehaviour
{
    public float minAngle;
    public float maxAngle;
    public float offset;

    void Update()
    {
        float delta = maxAngle / transform.childCount;

        foreach (Transform item in transform)
        {
            Vector3 direction = Vector3.zero;
            direction = Quaternion.Euler(0, 0, item.GetSiblingIndex() * delta + minAngle) * transform.right;
            item.position = transform.position + direction * offset;
        }
    }
}
