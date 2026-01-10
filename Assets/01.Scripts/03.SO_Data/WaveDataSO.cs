using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("WaveData/SO"))]
public class WaveDataSO : ScriptableObject
{
    public GameObject enemyPrefab;
    public int count;             
    public float spawnInterval;   
}
