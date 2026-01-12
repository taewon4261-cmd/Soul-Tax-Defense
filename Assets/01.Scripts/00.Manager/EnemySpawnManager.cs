using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;

    void EnemySpawn()
    {
        float x = 11;
        float y = Random.Range(-1, 6);

        Vector2 pos = new Vector2(x, y);

        Instantiate(enemyPrefab, pos, Quaternion.identity);
    }

    async UniTask Respawn()
    {
        while (true)
        {
            EnemySpawn();
            await UniTask.Delay(3000, cancellationToken: this.GetCancellationTokenOnDestroy());
        }
       
    }

    private void Start()
    {
        Respawn().Forget();
    }
}
