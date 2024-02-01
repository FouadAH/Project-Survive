using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerAbilityDataSO : ScriptableObject
{
    [Header("Health")]
    public float baseHealth;
    public float maxHealth;
    public float healthMod;

    public float MaxHealth { get => baseHealth + healthMod; }

    [Header("Mana")]
    public float baseMana;
    public float maxMana;
    public float manaMod;
    public float Mana { get => baseMana + manaMod; }

    [Header("Speed")]
    public float baseSpeed;
    public float maxSpeed;
    public float speedMod;
    public float Speed { get => baseSpeed + speedMod; }

    public float dashSpeedMod;
    public float launchSpeedMod;
    public float launchDamageMod;

    [Header("Abilities")]
    public bool hasDash;
    public bool hasHover;
    public bool hasTelekenises;

    [Header("Damage")]
    public float damageMod;

    [Header("Currency")]
    public float currency;


    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        hasDash = false;
        hasHover = false;
        hasTelekenises = false;
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

    public void SetDashSpeedMod(float modifier, int level)
    {
        dashSpeedMod = modifier * level;
    }
}
