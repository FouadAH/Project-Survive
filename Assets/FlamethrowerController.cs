using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FlamethrowerController : MonoBehaviour
{
    public ParticleSystem flamethrowerPS;
    public DamageSource damageSourcePerTick;
    public LayerMask damageMask;
    public float attackCooldown;
    private float attackCooldownCurrent;
    private MeshCollider meshCollider;
    bool isActive;

    private void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
    }

    private void Start()
    {
        ToggleActivateDamageTrigger(false);
    }

    private void Update()
    {
        if (attackCooldownCurrent > 0)
        {
            attackCooldownCurrent -= Time.deltaTime;
        }
        else
        {
            attackCooldownCurrent = 0;
        }
    }

    public void ToggleActivateDamageTrigger(bool state)
    {
        meshCollider.enabled = state;

        if (state)
        {
            flamethrowerPS.Play();
        }
        else
        {
            flamethrowerPS.Stop();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other == null)
            return;


        if (attackCooldownCurrent != 0)
            return;

        attackCooldownCurrent = attackCooldown;

        var damageable = other.GetComponent<DamagableBase>();
        if (damageable != null && Utility.IsInLayerMask(damageMask, other.gameObject.layer))
        {
            Vector3 direction = damageable.transform.position - transform.position;
            float distance = Mathf.Clamp(direction.magnitude, 0f, 20f);
            distance = Utility.Remap(distance, 0f, 20f, 3f, 1f);
            damageable.TakeDamage(damageSourcePerTick.damageValue * distance, direction, 100);
        }
    }

}
