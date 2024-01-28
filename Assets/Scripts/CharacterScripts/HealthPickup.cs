using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : PickupBase
{
    public PlayerDataSO playerDataSO;
    public float healing;

    public override void OnPickUp(DamageController damageController)
    {
        base.OnPickUp(damageController);
        damageController.Heal(healing);
        Destroy(gameObject);
    }
}
