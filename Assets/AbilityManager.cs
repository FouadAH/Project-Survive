using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public const int MAX_LEVEL = 5;
    public static AbilityManager instance;
    public List<AbilityItemSO> abilityItems;
    public List<AbilityItemSO> abilitiesGained = new List<AbilityItemSO>();

    public WeaponSwitchController weaponSwitchController;
    public PlayerAbilityDataSO playerAbilityDataSO;
    public GunDataSO shotgun;
    public GunDataSO rifle;
    public GunDataSO gun;


    System.Random _random = new System.Random();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            _random = new System.Random();
            abilitiesGained = new List<AbilityItemSO>();
        }

        gun.isAvailable = true;
        weaponSwitchController.InitList();
    }

    public List<AbilityItemSO> GetRandomAbility(int abilityCount)
    {
        //int random = Random.Range(0, abilityItems.Count);
        List<AbilityItemSO> abilities = new List<AbilityItemSO>();

        var abilitiesList = abilityItems.Where((ability) => 
            !(ability.isUnique && ability.hasObtainedAbility) 
            && ability.level < MAX_LEVEL 
            && abilitiesGained.Intersect(ability.prereq).ToList().Count >= ability.prereq.Count
        );

        for (int i = 0; i < abilityCount; i++)
        {
            if (abilitiesList.Except(abilities).ToArray().Length > 0)
            {
                abilities.Add(GetRandomItem(abilitiesList.Except(abilities), x => x.weight));
            }
        }
       
        return abilities;
    }

    public T GetRandomItem<T>(IEnumerable<T> itemsEnumerable, Func<T, int> weightKey)
    {
        var items = itemsEnumerable.ToList();

        var totalWeight = items.Sum(x => weightKey(x));
        var randomWeightedIndex = _random.Next(totalWeight);
        var itemWeightedIndex = 0;
        foreach (var item in items)
        {
            itemWeightedIndex += weightKey(item);
            if (randomWeightedIndex < itemWeightedIndex)
                return item;
        }
        throw new ArgumentException("Collection count and weights must be greater than 0");
    }
    public void OnReceivedAbility(AbilityItemSO abilityItemSO)
    {
        abilitiesGained.Add(abilityItemSO);
        abilityItemSO.level++;
        abilityItemSO.hasObtainedAbility = true;
        abilityItemSO.weight -= Mathf.Clamp(abilityItemSO.weight - 10 * abilityItemSO.level, 5, 100);
        playerAbilityDataSO.currency -= abilityItemSO.FinalCost;
        weaponSwitchController.InitList();
        //switch (abilityItemSO.abilityID) 
        //{
        //    case 100:
        //        rifle.isAvailable = true;
        //        weaponSwitchController.InitList();
        //        break;

        //    case 101:
        //        shotgun.isAvailable = true;
        //        weaponSwitchController.InitList();
        //        break;

        //    case 102:
        //        playerAbilityDataSO.hasTelekenises = true;
        //        break;

        //    case 103:
        //        playerAbilityDataSO.hasDash = true;
        //        break;

        //    case 104:
        //        playerAbilityDataSO.hasHover = true;
        //        break;

        //    case 105:
        //        playerAbilityDataSO.maxHealth = playerAbilityDataSO.baseHealth + 5 * abilityItemSO.level;
        //        break;

        //    case 106:
        //        playerAbilityDataSO.speedMod = 2 * abilityItemSO.level;
        //        break;

        //    case 107:
        //        playerAbilityDataSO.damageMod =  5 * abilityItemSO.level;
        //        break;
        //}
    }
}
