using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBase : MonoBehaviour
{
    public LayerMask playerMask;

    public virtual void OnPickUp() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if(Utility.IsInLayerMask(playerMask, other.gameObject.layer))
            {
                OnPickUp();
            }
        }
    }
}
