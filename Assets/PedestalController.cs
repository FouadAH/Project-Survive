using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PedestalController : MonoBehaviour
{
    public AbilityItemSO abilityDataSO;
    public AbilityItem abilityItemUI;

    public LayerMask player;
    private void OnTriggerEnter(Collider other)
    {
        if (Utility.IsInLayerMask(player, other.gameObject.layer))
        {
            abilityItemUI.Init(abilityDataSO);
            abilityItemUI.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Utility.IsInLayerMask(player, other.gameObject.layer))
        {
            abilityItemUI.gameObject.SetActive(false);
        }
    }
}
