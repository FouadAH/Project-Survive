using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    public Slider slider;
    public TMPro.TMP_Text healthText;
    public float sliderValue;
    public bool billboard = true;
    bool isShaking;

    private void Update()
    {
        slider.value = sliderValue;

        healthText.text = $"{slider.value}/{slider.maxValue}";

        if (billboard)
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }

    public void DamageAnimation()
    {
        if (!isShaking)
        {
            isShaking = true;
            transform.DOShakeScale(0.2f, 0.8f).OnComplete(() =>
            {
                isShaking = false;
            });
        }
    }
    public void SetMax(float maxValue)
    {
        slider.maxValue = maxValue;
    }
}
