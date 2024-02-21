using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingVolumeController : MonoBehaviour
{
    [Header("Damage Effect")]
    public float damageEffectTime;
    public Color targetColor_Damage;
    public float targetIntensity_Damage;

    [Header("Healing Effect")]
    public float healingEffectTime;
    public Color targetColor_Healing;
    public float targetIntensity_Healing;

    [Header("Heal Effect")]
    public float healEffectTime;
    public Color targetColor_Heal;
    public float targetIntensity_Heal;

    [Header("Normal Effect")]
    public float normalIntensity;
    public Color normalColor;

    private Volume volume;

    private ChromaticAberration chromaticAberration;
    private Vignette vignette;

    private float currentTime;

    private bool isPlayingEffect_Damage;
    private bool isPlayingEffect_Heal;
    private bool isPlayingEffect_Healing;

    private void Start()
    {
        volume = GetComponent<Volume>();

        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out chromaticAberration);
    }

    public void DamageEffect()
    {
        if(!isPlayingEffect_Damage)
        {
            isPlayingEffect_Damage = true;
            StartCoroutine(
                EffectRoutine(damageEffectTime, targetIntensity_Damage, targetColor_Damage, normalIntensity, normalColor, () =>
                { 
                    isPlayingEffect_Damage = false; 
                })
            );
        }
    }
    public void HealEffect()
    {
        if (!isPlayingEffect_Heal)
        {
            isPlayingEffect_Heal = true;
            StartCoroutine(
                EffectRoutine(healEffectTime, targetIntensity_Heal, targetColor_Heal, normalIntensity, normalColor, () =>
                {
                    isPlayingEffect_Heal = false;
                })
            );
        }
    }

    IEnumerator EffectRoutine(float effectTime, float intensityStart, Color colorStart, float intensityEnd, Color colorEnd, Action OnEnd = null)
    {
        currentTime = 0;

        while (currentTime < effectTime)
        {
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, intensityStart, (currentTime / damageEffectTime));
            vignette.color.value = Color.Lerp(vignette.color.value, colorStart, (currentTime / damageEffectTime));

            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, intensityStart, (currentTime / damageEffectTime));
            currentTime += Time.deltaTime;
            yield return null;
        }

        currentTime = 0;
        while (currentTime < effectTime)
        {
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, intensityEnd, (currentTime / damageEffectTime));
            vignette.color.value = Color.Lerp(vignette.color.value, colorEnd, (currentTime / damageEffectTime));

            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, 0, (currentTime / damageEffectTime));
            currentTime += Time.deltaTime;
            yield return null;
        }

        OnEnd?.Invoke();
    }
}
