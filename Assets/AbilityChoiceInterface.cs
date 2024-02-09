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
    public PlayerInput playerInput;
    private void OnEnable()
    {
        playerInput.currentActionMap.Disable();
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
        playerInput.currentActionMap.Enable();

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
                abilityItemUI.OnReceived = (() =>
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
