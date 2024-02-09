using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyPickup : PickupBase
{
    public int currency;
    public override void Update()
    {
        base.Update();
        transform.Rotate(180 * Time.deltaTime, 0, 0);
    }

    public override void OnPickUp(PlayerDamageController damageController)
    {
        base.OnPickUp(damageController);
        damageController.GetComponent<PlayerInputController>().playerAbilityDataSO.currency += currency;
        Destroy(transform.parent.gameObject);
    }
}
