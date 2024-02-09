using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerAbilityDataSO : ScriptableObject
{
    [Header("Health")]
    public float baseHealth;
    public float healthMod;
    public float MaxHealth { get => baseHealth + healthMod; }

    [Header("Mana")]
    public float baseMana;
    public float manaMod;
    public float MaxMana { get => baseMana + manaMod; }

    [Header("Healing")]
    public float baseHealing;
    public float maxHealing;
    public float healingMod;
    public float Healing { get => baseHealing + healingMod; }

    [Header("Healing Cost")]
    public float healingManaCostBase;
    public float minHealingCost;
    public float healingManaCostMod;
    public float HealingCost { get => healingManaCostBase + healingManaCostMod; }

    [Header("Speed")]
    public float baseSpeed;
    public float maxSpeed;
    public float speedMod;
    public float Speed { get => baseSpeed + speedMod; }

    public float dashSpeedMod;

    [Header("Launch Speed")]
    public float launchSpeedBase;
    public float launchSpeedMod;
    public float LaunchSpeed { get => launchSpeedBase + launchSpeedMod; }

    [Header("Launch Damage")]
    public float launchDamageBase;
    public float launchDamageMod;
    public float LaunchDamage { get => launchDamageBase + launchDamageMod; }

    [Header("Abilities")]
    public bool hasDash;
    public bool hasHover;
    public bool hasTelekenises;

    [Header("Damage")]
    public float damageMod;

    [Header("Currency")]
    public int currency;


    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        hasDash = false;
        hasHover = false;
        hasTelekenises = false;

        speedMod = 0;
        dashSpeedMod = 0;
        launchSpeedMod = 0;
        launchDamageMod = 0;
        damageMod = 0;
        healthMod = 0;
        manaMod = 0;

        currency = 0;
    }

    public void DashAbility() 
    {
        hasDash = true;
    }

    public void TelekAbility()
    {
        hasTelekenises = true;
    }

    public void HoverAbility()
    {
        hasHover = true;
    }

    public void SetHealthMod(float modifier, int level)
    {
        healthMod = modifier * level;
    }

    public void SetSpeedMod(float modifier, int level)
    {
        speedMod = modifier * level;
    }

    public void SetMaxManaMod(float modifier, int level)
    {
        manaMod = modifier * level;
    }

    public void SetLaunchSpeedMod(float modifier, int level)
    {
        launchSpeedMod = modifier * level;
    }

    public void SetLaunchDamageMod(float modifier, int level)
    {
        launchDamageMod = modifier * level;
    }

    public void SetDamageMod(float modifier, int level)
    {
        damageMod = modifier * level;
    }

    public void SetDashSpeedMod(float modifier, int level)
    {
        dashSpeedMod = modifier * level;
    }
}
