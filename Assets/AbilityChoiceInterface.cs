using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityChoiceInterface : MonoBehaviour
{
    public WaveManager waveManager;
    public AbilityItem abilityItem;
    public Transform content;
    public int abilityCount = 3;
    private void OnEnable()
    {
        Time.timeScale = Mathf.Epsilon;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        GetRandomAbilities();
    }

    private void OnDisable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;

        waveManager.StartWave();
    }

    void GetRandomAbilities()
    {
        foreach (Transform item in content)
        {
            Destroy(item.gameObject);
        }

        int count = Mathf.Min(abilityCount, AbilityManager.instance.abilityItems.Count);

        if (count > 0)
        {
            var abilityDataList = AbilityManager.instance.GetRandomAbility(count);
            foreach (AbilityItemSO abilityData in abilityDataList)
            {
                var abilityItemUI = Instantiate(abilityItem, content);
                abilityItemUI.Init(abilityData);
                abilityItemUI.button.onClick.AddListener(() =>
                {
                    gameObject.SetActive(false);
                });
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
