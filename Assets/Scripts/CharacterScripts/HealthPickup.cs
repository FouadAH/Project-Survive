using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : PickupBase
{
    public float healing;

    public override void OnPickUp(PlayerDamageController damageController)
    {
        base.OnPickUp(damageController);
        damageController.RecoverMana(healing);
        Destroy(transform.parent.gameObject);
    }
}
