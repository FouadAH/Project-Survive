using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourBase : MonoBehaviour
{
    protected ContextMap contextMap;

    private void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        contextMap = GetComponent<ContextMap>();
    }

    public virtual float[] ConstructInterestMap() 
    {
        return null;
    }

    public virtual float[] ConstructDangerMap()
    {
        return null;
    }

    public void FillMap(float[] map, int resolution, int index, float weight, float falloffRate)
    {
        map[index] = weight;

        for (int i = 1; i <= resolution; i++)
        {
            map[(index + i) % map.Length] = Mathf.Clamp(weight - falloffRate * i, 0, Mathf.Infinity);

            int remapedIndex = index - i;
            if (remapedIndex < 0)
            {
                remapedIndex = (int)Utility.Remap(index - i, -1f, (map.Length) * -1, map.Length - 1, 0f);
            }

            map[remapedIndex] = Mathf.Clamp(weight - falloffRate * i, 0, Mathf.Infinity);
        }
    }
}
