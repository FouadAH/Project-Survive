using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AbilityItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Data")]
    public PlayerAbilityDataSO playerData;
    public AbilityItemSO abilityItemSO;
    public LevelColorsSO LevelColorsSO;

    [Header("UI Elements")]
    public Image panelImage;
    public Image borderImage;

    public Button button;
    public TMPro.TMP_Text abilityName;
    public TMPro.TMP_Text abilityLevelText;
    public TMPro.TMP_Text abilityCostText;

    public TMPro.TMP_Text abilityDescription;
    public Transform statPanel;
    public Slider holdSlider;
    public float requiredHoldTime;

    public Action OnReceived;

    float holdTime;
    bool isHeld;
    bool hasReceived;

    private void Start()
    {
        holdSlider.maxValue = requiredHoldTime;
    }
    private void OnEnable()
    {
        hasReceived = false;
    }

    private void Update()
    {
        if (!button.interactable)
            return;

        if (isHeld)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                holdTime += 0.05f;
                if (holdTime > requiredHoldTime && !hasReceived)
                {
                    hasReceived = true;
                    abilityItemSO.OnReceivedAbility();
                    gameObject.SetActive(false);
                    OnReceived?.Invoke();
                }
            }
            else
            {
                holdTime = 0;
            }
        }
        else
        {
            holdTime = 0;
        }

        holdSlider.value = holdTime;
    }

    public void Init(AbilityItemSO abilityItemSO)
    {
        this.abilityItemSO = abilityItemSO;
        abilityName.text = abilityItemSO.abilityName;
        abilityLevelText.text = $"LVL: {abilityItemSO.level}";
        abilityCostText.text = $"Cost: {abilityItemSO.FinalCost}";

        abilityDescription.text = abilityItemSO.abilityDescription;

        button.interactable = playerData.currency >= abilityItemSO.FinalCost;
        panelImage.color = LevelColorsSO.colors[abilityItemSO.level];
        borderImage.color = LevelColorsSO.colors[abilityItemSO.level];

        if (abilityItemSO.isUnique)
        {
            panelImage.color = LevelColorsSO.colors[LevelColorsSO.colors.Count - 1];
            borderImage.color = LevelColorsSO.colors[LevelColorsSO.colors.Count - 1];
        }
    }

    public void OnClick()
    {
        //abilityItemSO.OnReceivedAbility();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHeld = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHeld = false;
    }
}

