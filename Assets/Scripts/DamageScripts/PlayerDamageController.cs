using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageController : DamageController
{
    public PlayerAbilityDataSO playerAbilityDataSO;
    public InputProvider inputProvider;

    public HealthSlider manaSlider;

    public float currentMana;

    private void OnEnable()
    {
        inputProvider.HealEvent += OnHeal;

        currentMana = playerAbilityDataSO.MaxMana;
    }

    private void OnDisable()
    {
        inputProvider.HealEvent -= OnHeal;
    }

    public void RecoverMana(float manaRecovered)
    {
        currentMana = Mathf.Clamp(currentMana + manaRecovered, 0, playerAbilityDataSO.MaxMana);
    }
    public void SpendMana(float manaRecovered)
    {
        currentMana = Mathf.Clamp(currentMana - manaRecovered, 0, playerAbilityDataSO.MaxMana);
    }

    void OnHeal()
    {
        Debug.Log("heal");
        if(currentMana >= playerAbilityDataSO.HealingCost)
        {
            SpendMana(playerAbilityDataSO.HealingCost);
            Heal(playerAbilityDataSO.Healing);
        }
    }

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
    }
}
