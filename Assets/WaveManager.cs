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

    [Header("Canvas Settings")]
    public TMPro.TextMeshPro waveTimerText;

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
        waveTimerText.text = currentWaveTime.ToString();

    }

    public void StartWave()
    {
        currentWaveCount++;
        currentWaveTime = waveTime;

        foreach (var spawner in spawnControllers)
        {
            spawner.ConstructSpawnQueue();
        }
    }
}
