using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitchController : MonoBehaviour
{
    public GunController gunController;

    public List<GunItem> gunItems = new List<GunItem>();
    public GunItem currentItem;
    
    private int currentIndex;

    private void Start()
    {
        currentItem = gunItems[currentIndex];
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
        currentIndex = (down) ? (currentIndex + 1) % gunItems.Count  : Mathf.Abs(currentIndex - 1) % gunItems.Count;
        currentItem = gunItems[currentIndex];

        gunController.SetActiveGun(currentItem);
    }
}
