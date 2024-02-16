using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSetter : MonoBehaviour
{
    public Material effectMaterial;

    [ContextMenu("Set Material")]
    public void SetMaterial()
    {
        MeshRenderer[] meshFilters = GetComponentsInChildren<MeshRenderer>();
        int i = 0;
        while (i < meshFilters.Length)
        {
            meshFilters[i].material = effectMaterial;
            i++;
        }
    }

    public void SetEffectStrength(float strength)
    {
        MeshRenderer[] meshFilters = GetComponentsInChildren<MeshRenderer>();
        int i = 0;
        while (i < meshFilters.Length)
        {
            meshFilters[i].material.SetFloat("_EffectStrength", strength);
            i++;
        }
        //effectMaterial.SetFloat("_EffectStrength", strength);
    }
}
