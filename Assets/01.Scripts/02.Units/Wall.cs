using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public static Wall instance;

    public int hp;

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        Debug.Log($"성벽 남은 체력 : {hp}");
    }
}
