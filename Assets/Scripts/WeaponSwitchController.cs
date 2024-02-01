using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitchController : MonoBehaviour
{
    public GunController gunController;

    public List<GunItem> gunItems = new List<GunItem>();
    public List<GunItem> availableGunItems = new List<GunItem>();

    public GunItem currentItem;
    
    private int currentIndex;

    private void Start()
    {
        InitList();
        currentItem = availableGunItems[currentIndex];
        foreach (var item in gunItems)
        {
            item.gunDataSO.OnSetAvailable += InitList;
        }
    }

    private void OnDestroy()
    {
        foreach (var item in gunItems)
        {
            item.gunDataSO.OnSetAvailable -= InitList;
        }
    }

    public void InitList()
    {
        availableGunItems = gunItems.Where((gun) => gun.gunDataSO.isAvailable == true).ToList();
    }
    public void OnSwitch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            var normalizedInput = context.ReadValue<float>();
            Switch(normalizedInput > 0);
        }
    }

    public void Switch(bool down)
    {
        currentIndex = (down) ? (currentIndex + 1) % availableGunItems.Count : Mathf.Abs(currentIndex - 1) % availableGunItems.Count;

        if (currentItem.gunDataSO.isAvailable)
        {
            currentItem = availableGunItems[currentIndex];
            gunController.SwitchActiveGun(currentItem);
        }
        else
        {
            Switch(down);
        }
    }
}
