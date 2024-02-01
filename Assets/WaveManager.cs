using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public SpawnController[] spawnControllers;

    [Header("Wave Settings")]
    public float waveTime = 60;
    public int minWaveEntityCount = 30;
    public int maxWaveEntityCount = 50;

    public float baseTimeMod;
    public float baseEntityCountMod;

    public float baseSpawnerCurrencyMod;


    [Header("Canvas Settings")]
    public TMPro.TMP_Text waveTimerText;
    public CanvasGroup abilityChoiceCanvas;

    private float currentWaveTime;
    private int currentEntityCount;
    private int currentWaveCount;

    private void Awake()
    {
        spawnControllers = GetComponentsInChildren<SpawnController>();
    }

    private void Start()
    {
        StartWave();
    }

    private void Update()
    {
        currentEntityCount = EntityManager.Instance.entities.Count;
        waveTimerText.text = Mathf.Ceil(currentWaveTime).ToString();
    }

    public void StartWave()
    {
        StartCoroutine(WaveRoutine());
    }

    public void EndWave()
    {
        abilityChoiceCanvas.gameObject.SetActive(true);
    }

    IEnumerator WaveRoutine()
    {
        abilityChoiceCanvas.gameObject.SetActive(false);

        currentWaveCount++;
        currentWaveTime = waveTime + (baseTimeMod * currentWaveCount);

        foreach (var spawner in spawnControllers)
        {
            spawner.maxSpawnCount += (int)(baseEntityCountMod * currentWaveCount);
            spawner.spawnCurrency += (int)(baseSpawnerCurrencyMod * currentWaveCount);
            spawner.ConstructSpawnQueue();
        }

        while (currentWaveTime > 0) 
        {
            currentWaveTime -= Time.deltaTime;
            yield return null;
        }
        EndWave();
    }
}
