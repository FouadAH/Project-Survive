using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public WaveManager waveManager;
    public List<PedestalController> pedestals;
    public GameObject pedestalParent;
    public void InitShop()
    {
        pedestalParent.gameObject.SetActive(true);
        int count = Mathf.Min(pedestals.Count, AbilityManager.instance.abilityItems.Count);

        if (count > 0)
        {
            var abilityDataList = AbilityManager.instance.GetRandomAbility(count);

            int index = 0;
            foreach (AbilityItemSO abilityData in abilityDataList)
            {
                pedestals[index].abilityDataSO = abilityData;
                pedestals[index].abilityItemUI.OnReceived = (() =>
                {
                    pedestalParent.SetActive(false);
                    waveManager.StartWave();
                });

                index++;
            }
        }
    }
}
