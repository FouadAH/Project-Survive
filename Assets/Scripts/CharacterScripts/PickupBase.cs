using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBase : MonoBehaviour
{
    public LayerMask playerMask;

    public virtual void OnPickUp(DamageController damageController) { }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if(Utility.IsInLayerMask(playerMask, other.gameObject.layer))
            {
                DamageController damageController = other.GetComponent<DamageController>();
                OnPickUp(damageController);
            }
        }
    }
}
