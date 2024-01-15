using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="States/PullStateData")]
public class PulledStateData : ScriptableObject
{
    public float maxPullDistance;
    public float pullSpeed;
    public float pullVelocitySmoothing;
}
