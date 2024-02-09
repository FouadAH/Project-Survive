using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttractor : MonoBehaviour
{
    public LayerMask particleMask;

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (Utility.IsInLayerMask(particleMask, other.gameObject.layer))
            {
                AttractionController attractionController = other.GetComponent<AttractionController>();
                if(attractionController != null)
                {
                    attractionController.isAttracted = true;
                }
            }
        }
    }
}
