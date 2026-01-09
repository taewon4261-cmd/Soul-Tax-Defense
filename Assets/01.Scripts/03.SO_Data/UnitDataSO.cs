using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Unit/SO"))]
public class UnitDataSO : ScriptableObject
{
    public string unitName;
    public int cost;
    public int maxHp;
    public int attackPower;

    public float attackSpeed;
    public float attackRange;
    public Sprite unitSprite;

}
