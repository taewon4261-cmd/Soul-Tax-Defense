using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("Wave Settings")]
    public List<WaveDataSO> waves;
    public float xSpawnPos = 11f; 

    [Header("Status")]
    public int currentWaveIndex = 0;
    private int enemiesAlive = 0;
    private int enemiesToSpawn = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void StartWave()
    {
        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log(" 모든 웨이브 클리어!");
            return;
        }

        SpawnWaveRoutine().Forget();
    }

    private async UniTaskVoid SpawnWaveRoutine()
    {
        WaveDataSO currentWave = waves[currentWaveIndex];

        enemiesToSpawn = currentWave.count;
        enemiesAlive = currentWave.count;

        Debug.Log($"Wave {currentWaveIndex + 1} 시작! 총 {enemiesToSpawn}마리");

        for (int i = 0; i < currentWave.count; i++)
        {
            SpawnEnemy(currentWave.enemyPrefab);
            enemiesToSpawn--;
            await UniTask.Delay((int)(currentWave.spawnInterval * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
        }

        currentWaveIndex++; 
    }

    void SpawnEnemy(GameObject prefab)
    {
        float randomY = Random.Range(-1, 6); 
        Vector2 pos = new Vector2(xSpawnPos, randomY);

        Instantiate(prefab, pos, Quaternion.identity);
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;
        if (enemiesAlive < 0) enemiesAlive = 0;

        Debug.Log($"적 처치! 남은 적: {enemiesAlive}");
    }

    public bool IsWaveFinished()
    {
        return enemiesToSpawn <= 0 && enemiesAlive <= 0;
    }
}