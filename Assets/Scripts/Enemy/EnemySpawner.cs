using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPhase
    {
        public float timeToActivate;
        public string[] enemyPoolTags;
        public float spawnInverval;
        public int maxEnemiesOnScreen;
    }

    [SerializeField] private float maxGameTime = 240f;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private SpawnPhase[] phases;
    [SerializeField] private float initialDelay = 5f;

    private int currentPhaseIndex;
    private float gameTimer;
    private float nextSpawnTime;
   
    private List<GameObject> activeEnemies = new();

    private void Start()
    {
        nextSpawnTime = Time.time + initialDelay;
    }

    private void Update()
    {
        if (gameTimer >= maxGameTime) return;
        
        gameTimer += Time.deltaTime;
        
        activeEnemies.RemoveAll(item => item == null || !item.activeInHierarchy);

        UpdatePhase();
        HandleSpawning();
    }

    private void UpdatePhase()
    {
        if(currentPhaseIndex < phases.Length - 1)
        {
            if(gameTimer >= phases[currentPhaseIndex + 1].timeToActivate)
            {
                currentPhaseIndex++;
            }
        }
    }

    private void HandleSpawning()
    {
        SpawnPhase currentPhase = phases[currentPhaseIndex];
        if (activeEnemies.Count >= currentPhase.maxEnemiesOnScreen) return;

        if(Time.time >= nextSpawnTime)
        {
            SpawnEnemy(currentPhase);
            nextSpawnTime = Time.time + currentPhase.spawnInverval;
        }
    }

    private void SpawnEnemy(SpawnPhase phase)
    {
        if(spawnPoints.Length == 0 || phase.enemyPoolTags.Length == 0) return;
        Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        string randomTag = phase.enemyPoolTags[Random.Range(0, phase.enemyPoolTags.Length)];

        GameObject spawnedEnemy = PoolManager.Instance.SpawnFromPool(randomTag, randomPoint.position, Quaternion.identity);

        if(spawnedEnemy != null)
        {
            activeEnemies.Add(spawnedEnemy);
        }
    }
}

