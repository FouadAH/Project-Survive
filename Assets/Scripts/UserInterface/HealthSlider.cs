using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    public Slider slider;
    public TMPro.TMP_Text healthText;
    public float sliderValue;

    private void Update()
    {
        slider.value = sliderValue;

        healthText.text = $"{slider.value}/{slider.maxValue}";
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    public void SetMax(float maxValue)
    {
        slider.maxValue = maxValue;
    }
}
