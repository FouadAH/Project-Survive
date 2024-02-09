using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBase : MonoBehaviour
{
    public LayerMask playerMask;
    public virtual void OnPickUp(PlayerDamageController damageController) { }

    public virtual void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if(Utility.IsInLayerMask(playerMask, other.gameObject.layer))
            {
                PlayerDamageController damageController = other.GetComponent<PlayerDamageController>();
                OnPickUp(damageController);
            }
        }
    }
}
