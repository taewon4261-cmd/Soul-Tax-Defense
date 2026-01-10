using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveData
{
    public GameObject enemyPrefab; // 소환할 적
    public int count;              // 몇 마리?
    public float spawnInterval;    // 몇 초 간격?
}

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("Settings")]
    public Transform spawnPoint;   // 적이 나오는 위치 (우측)
    public List<WaveData> waves;   // 인스펙터에서 설정할 웨이브 목록

    [Header("Status")]
    public int currentWaveIndex = 0;
    private int enemiesAlive = 0;      // 현재 살아있는 적
    private int enemiesToSpawn = 0;    // 앞으로 나와야 할 적
    private bool isSpawning = false;   // 스폰 코루틴이 도는 중인가?

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // 밤이 되면 호출됨
    public void StartWave()
    {
        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("모든 웨이브 클리어!");
            return;
        }

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        isSpawning = true;
        WaveData wave = waves[currentWaveIndex];

        enemiesToSpawn = wave.count;
        enemiesAlive = wave.count; // 나올 적 = 죽여야 할 적

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemyPrefab);
            enemiesToSpawn--;
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        isSpawning = false; // 스폰은 끝남 (하지만 적은 살아있을 수 있음)
        currentWaveIndex++; // 다음 웨이브 준비
    }

    void SpawnEnemy(GameObject prefab)
    {
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }

    // 적이 죽을 때 EnemyBase에서 이 함수를 호출해줘야 함! (중요)
    public void OnEnemyKilled()
    {
        enemiesAlive--;
        if (enemiesAlive < 0) enemiesAlive = 0;
    }

    // GameManager가 매 프레임 물어볼 함수
    public bool IsWaveFinished()
    {
        // 스폰도 다 끝났고(0) && 살아있는 적도 없으면(0) -> True
        return enemiesToSpawn == 0 && enemiesAlive == 0;
    }


}
