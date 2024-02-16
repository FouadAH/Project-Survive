using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    public Slider slider;
    public Slider ghostSlider;

    public TMPro.TMP_Text healthText;
    public float sliderValue;
    public bool billboard = true;
    float lastValue;
    bool isShaking;
    bool isHidden;
    private void Update()
    {
        lastValue = slider.value;
        slider.value = sliderValue;
        ghostSlider.value = Mathf.Lerp(ghostSlider.value, sliderValue, 0.05f);
        healthText.text = $"{Mathf.CeilToInt(slider.value)}/{slider.maxValue}";

        if (billboard)
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }

        if(lastValue == slider.value && !isHidden)
        {
            //hide
        }
        else if(lastValue != slider.value&& isHidden) 
        {
            //show
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
        ghostSlider.maxValue = maxValue;
    }
}
