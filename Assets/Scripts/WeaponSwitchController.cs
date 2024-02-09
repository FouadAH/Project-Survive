using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WeaponSwitchController : MonoBehaviour
{
    public GunController gunController;

    public List<GunItem> gunItems = new List<GunItem>();
    public List<GunItem> availableGunItems = new List<GunItem>();
    public List<GameObject> gunUIs = new List<GameObject>();

    public GunItem currentItem;
    public CircleLayout circleLayout;
    public GameObject gunUIItem;

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

        foreach (Transform item in circleLayout.transform)
        {
            Destroy(item.gameObject);
        }
        gunUIs.Clear();
        foreach (var gunItem in availableGunItems)
        {
            var item =Instantiate(gunUIItem, circleLayout.transform);
            item.GetComponent<Image>().color = gunItem.gunDataSO.color;
            gunUIs.Add(item);
        }
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
        currentIndex = (down) ? Mathf.Abs(currentIndex + 1) % availableGunItems.Count : Mathf.Abs((currentIndex - 1) % availableGunItems.Count);

        if (currentItem.gunDataSO.isAvailable)
        {
            currentItem = availableGunItems[currentIndex];
            gunController.SwitchActiveGun(currentItem);
        }
        else
        {
            Switch(down);
        }

        foreach (Transform item in circleLayout.transform)
        {
            item.localScale = Vector3.one;
        }

        gunUIs[currentIndex].transform.localScale = Vector3.one * 1.2f;
    }
}
