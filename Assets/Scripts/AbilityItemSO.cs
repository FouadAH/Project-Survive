using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class AbilityItemSO : ScriptableObject
{
    public int abilityID;
    public string abilityName;
    [TextArea] public string abilityDescription;

    public bool hasObtainedAbility;
    public bool isUnique;

    public int level;
    public float baseModifier;

    public int weight;
    public int baseCost = 100;

    public List<AbilityEvent> modifiers;

    public int FinalCost { get => baseCost * level; }

    private void OnEnable()
    {
        level = 1;
        hasObtainedAbility = false;
    }

    public virtual void OnReceivedAbility()
    {
        AbilityManager.instance.OnReceivedAbility(this);
        foreach (var modifier in modifiers)
        {
            modifier?.Invoke(baseModifier, level);
        }
    }
}

[System.Serializable]
public class AbilityEvent : UnityEvent<float, int>
{
}
