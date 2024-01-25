using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    public DamageSource damageSource;
    public LayerMask damageMask;

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;

        var damageable = other.GetComponent<DamagableBase>();
        if(damageable != null && Utility.IsInLayerMask(damageMask, other.gameObject.layer))
        {
            Vector3 direction = damageable.transform.position - transform.position;
            float distance = Mathf.Clamp(direction.magnitude, 0f, 20f);
            distance = Utility.Remap(distance, 0f, 20f, 3f, 1f);
            damageable.TakeDamage(damageSource.damageValue * distance, direction, 100);
        }
    }
}
