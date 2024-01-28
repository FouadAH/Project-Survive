using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingVolumeController : MonoBehaviour
{
    public float damageEffectTime;
    public Color targetColor;

    public float targetIntensity;

    private Volume volume;

    private ChromaticAberration chromaticAberration;
    private Vignette vignette;

    private float currentTime;
    private bool isPlayingEffect_Damage;
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
            StartCoroutine(DamageEffectRoutine());
        }
    }

    IEnumerator DamageEffectRoutine()
    {
        currentTime = 0;

        while (currentTime < damageEffectTime) 
        {
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetIntensity, (currentTime / damageEffectTime));
            vignette.color.value = Color.Lerp(vignette.color.value, targetColor, (currentTime / damageEffectTime));

            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, targetIntensity, (currentTime / damageEffectTime));
            currentTime += Time.deltaTime;
            yield return null;
        }

        currentTime = 0;
        while (currentTime < damageEffectTime)
        {
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0, (currentTime / damageEffectTime));
            vignette.color.value = Color.Lerp(vignette.color.value, Color.black, (currentTime / damageEffectTime));

            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, 0, (currentTime / damageEffectTime));
            currentTime += Time.deltaTime;
            yield return null;
        }

        isPlayingEffect_Damage = false;
    }
}
