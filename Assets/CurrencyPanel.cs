using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyPanel : MonoBehaviour
{
    public PlayerAbilityDataSO playerAbilityData;

    public TMPro.TMP_Text currencyText;
    public TMPro.TMP_Text damageModText;
    public TMPro.TMP_Text speedModText;
    public TMPro.TMP_Text healthModText;


    private void Update()
    {
        currencyText.text = $"${playerAbilityData.currency}";
        damageModText.text = $"Damage Mod: {playerAbilityData.damageMod}";
        speedModText.text = $"Speed Mod: {playerAbilityData.speedMod}";
        healthModText.text = $"Max HP Mod: {playerAbilityData.healthMod}";
    }
}
