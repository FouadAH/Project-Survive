using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageController : DamageController
{
    public PlayerAbilityDataSO playerAbilityDataSO;
    private void Update()
    {
        InitMax(playerAbilityDataSO.MaxHealth);

        if (healthSlider != null)
        {
            healthSlider.sliderValue = health;
        }
    }
}
