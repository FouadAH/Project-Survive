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
                    StartCoroutine(Delay(attractionController));
                }
            }
        }
    }

    IEnumerator Delay(AttractionController attractionController)
    {
        yield return new WaitForSeconds(0.8f);
        attractionController.isAttracted = true;
    }
}
