using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Enemy/SO"))]
public class EnemyDataSO : ScriptableObject
{
    public int maxHp;
    public int goldReward;
    public int atk;
}
