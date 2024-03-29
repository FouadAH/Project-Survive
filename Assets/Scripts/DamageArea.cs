using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    public DamageSource damageSource;
    public DamageType damageType;
    public LayerMask damageMask;

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;

        var damageable = other.GetComponent<HitBox>();
        if(damageable != null && Utility.IsInLayerMask(damageMask, other.gameObject.layer))
        {
            Vector3 direction = damageable.transform.position - transform.position;
            float distance = Mathf.Clamp(direction.magnitude, 0f, 20f);
            distance = Utility.Remap(distance, 0f, 20f, 3f, 1f);
            damageable.DamageController.TakeDamage(damageSource.damageValue * distance, direction, 100, damageType);
        }
    }
}
