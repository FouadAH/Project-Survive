using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public SpawnController[] spawnControllers;

    [Header("Spawn Settings")]
    public int baseSpawnCurrency = 100;
    public float spawnRateBase = 3f;
    public float spawnRateMod = 3f;
    public AnimationCurve spawnTimeCurve;

    public EntitySpawnDataList spawnDataList = new EntitySpawnDataList();

    private float spawnCurrency;
    private float maxWaveEntityCount;

    [Header("Wave Settings")]
    public float waveTime = 60;
    public float maxEntityCount = 100;

    public int maxWaveEntityCountBase = 50;

    public float baseTimeMod;
    public float baseEntityCountMod;
    public AnimationCurve entityCountCurve;

    public float baseSpawnerCurrencyMod;

    [Header("Canvas Settings")]
    public TMPro.TMP_Text waveTimerText;
    public TMPro.TMP_Text roundCountText;
    public TMPro.TMP_Text entityCountText;

    public CanvasGroup abilityChoiceCanvas;
    public ShopController shopController;
    [Header("Debug Settings")]
    public bool debugMode;

    private float currentWaveTime;
    private int currentWaveCount;

    private int spawnCount = 0;
    private Queue<Entity> spawnQueue = new Queue<Entity>();

    private System.Random rand = new System.Random();

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
        waveTimerText.text = Mathf.Ceil(currentWaveTime).ToString();
        roundCountText.text = $"Round - {currentWaveCount}";
        entityCountText.text = $"Total enemies left - {EntityManager.Instance.entities.Count}";
    }

    public void StartWave()
    {
        StartCoroutine(WaveRoutine());
    }

    public void EndWave()
    {
        abilityChoiceCanvas.gameObject.SetActive(true);
        //shopController.InitShop();
    }

    IEnumerator ConstructSpawnQueueRoutine()
    {
        spawnQueue.Clear();

        spawnCurrency = baseSpawnCurrency + baseSpawnerCurrencyMod * currentWaveCount;
        maxWaveEntityCount = maxWaveEntityCountBase + baseEntityCountMod * currentWaveCount;
        spawnCount = 0;

        while (spawnCount < maxWaveEntityCount)
        {
            EntitySpawnData spawnData = GetRandomItem(spawnDataList.spawnData, x => (int)(x.spawnWeight * 100f));

            if (spawnCurrency >= spawnData.spawnCost && spawnCount < maxWaveEntityCount)
            {
                spawnQueue.Enqueue(spawnData.entity);
                spawnCurrency -= spawnData.spawnCost;
                spawnCount++;
                continue;
            }

            if (spawnCurrency <= 0 || spawnCount < maxWaveEntityCount || EntityManager.Instance.entities.Count >= maxEntityCount)
            {
                break;
            }

            yield return null;
        }

        yield return SpawnRoutine();
    }
    IEnumerator SpawnRoutine()
    {
        maxWaveEntityCount = Mathf.Clamp((int)entityCountCurve.Evaluate(currentWaveCount), 0, maxEntityCount);
        spawnCurrency = baseSpawnCurrency + baseSpawnerCurrencyMod * currentWaveCount;

        while (true)
        {
            bool canSpawn = EntityManager.Instance.entities.Count < maxWaveEntityCount;
            if (canSpawn)
            {
                EntitySpawnData spawnData = GetRandomItem(spawnDataList.spawnData, x => (int)(x.spawnWeight * 100f));

                int index = UnityEngine.Random.Range(0, spawnControllers.Length);
                var spawner = spawnControllers[index];
                spawner.Spawn(spawnData.entity);
                //Debug.Log(spawnTimeCurve.Evaluate(currentWaveCount));
            }

            float spawnInterval = spawnTimeCurve.Evaluate(currentWaveCount);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator WaveRoutine()
    {
        abilityChoiceCanvas.gameObject.SetActive(false);

        currentWaveCount++;
        currentWaveTime = waveTime + (baseTimeMod * currentWaveCount);

        StartCoroutine(SpawnRoutine());

        while (currentWaveTime > 0)
        {
            currentWaveTime -= Time.deltaTime;
            yield return null;
        }
        EndWave();
    }

    public T GetRandomItem<T>(IEnumerable<T> itemsEnumerable, Func<T, int> weightKey)
    {
        var items = itemsEnumerable.ToList();

        var totalWeight = items.Sum(x => weightKey(x));
        var randomWeightedIndex = rand.Next(totalWeight);
        var itemWeightedIndex = 0;
        foreach (var item in items)
        {
            itemWeightedIndex += weightKey(item);
            if (randomWeightedIndex < itemWeightedIndex)
                return item;
        }
        throw new ArgumentException("Collection count and weights must be greater than 0");
    }

}
