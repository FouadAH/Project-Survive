using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityItem : MonoBehaviour
{
    [Header("Data")]
    public PlayerAbilityDataSO playerData;
    public AbilityItemSO abilityItemSO;

    [Header("UI Elements")]
    public Button button;
    public TMPro.TMP_Text abilityName;
    public TMPro.TMP_Text abilityLevelText;
    public TMPro.TMP_Text abilityCostText;

    public TMPro.TMP_Text abilityDescription;
    public Transform statPanel;

    public void Init(AbilityItemSO abilityItemSO)
    {
        this.abilityItemSO = abilityItemSO;
        abilityName.text = abilityItemSO.abilityName;
        abilityLevelText.text = $"LVL: {abilityItemSO.level}";
        abilityCostText.text = $"Cost: {abilityItemSO.FinalCost}";

        abilityDescription.text = abilityItemSO.abilityDescription;

        button.interactable = playerData.currency >= abilityItemSO.FinalCost;
    }

    public void OnClick()
    {
        abilityItemSO.OnReceivedAbility();
    }
}

