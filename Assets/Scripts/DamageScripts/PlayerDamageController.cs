using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDamageController : DamageController
{
    public PlayerAbilityDataSO playerAbilityDataSO;
    public InputProvider inputProvider;

    [Header("Mana")]
    public HealthSlider manaSlider;
    public float currentMana;

    [Header("Debug")]
    public bool isGodMode;

    bool isHealing;

    private void OnEnable()
    {
        inputProvider.HealEvent_Start += OnHealStart;
        inputProvider.HealEvent_Stop += OnHealStop;
        currentMana = playerAbilityDataSO.MaxMana;
    }

    private void OnDisable()
    {
        inputProvider.HealEvent_Start -= OnHealStart;
        inputProvider.HealEvent_Stop -= OnHealStop;
    }

    public void RecoverMana(float manaRecovered)
    {
        currentMana = Mathf.Clamp(currentMana + manaRecovered, 0, playerAbilityDataSO.MaxMana);
    }
    public void SpendMana(float manaRecovered)
    {
        currentMana = Mathf.Clamp(currentMana - manaRecovered, 0, playerAbilityDataSO.MaxMana);
    }

    void OnHealStart()
    {
        if(currentMana > 0)
        {
            isHealing = true;
        }
    }

    void OnHealStop()
    {
        isHealing = false;
    }

    float totalSpentMana;
    private void Update()
    {
        health = Mathf.Clamp(health, 0, playerAbilityDataSO.MaxHealth);
        if (healthSlider != null)
        {
            healthSlider.SetMax(playerAbilityDataSO.MaxHealth);
            healthSlider.sliderValue = health;
        }

        if(manaSlider != null)
        {
            manaSlider.SetMax(playerAbilityDataSO.MaxMana);
            manaSlider.sliderValue = currentMana;
        }


        if (isHealing)
        {
            if(totalSpentMana > playerAbilityDataSO.HealingCost)
            {
                Heal(playerAbilityDataSO.Healing);
                totalSpentMana = 0;
                isHealing = false;
            }

            if (currentMana > 0)
            {
                float spentMana = Time.deltaTime * playerAbilityDataSO.HealingSpeed;
                totalSpentMana += spentMana;
                SpendMana(spentMana);
            }
        }
        else
        {
            RecoverMana(totalSpentMana);
            totalSpentMana = 0;
        }

        inputProvider.isHealing = isHealing;
    }

    public override void TakeDamage(float damageValue, Vector3 normal, float force = 1, DamageType damageType = DamageType.Range)
    {
        if (isGodMode)
        {
            damageValue = 0;
        }

        base.TakeDamage(damageValue, normal, force, damageType);
    }
}
