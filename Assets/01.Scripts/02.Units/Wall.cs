using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wall : MonoBehaviour
{
    public static Wall instance;

    public int hp;

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        Debug.Log($"성벽 남은 체력 : {hp}");
        Die();
    }

    void Die()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("ResultScene");
            Debug.Log("성벽이 무너졌습니다 패배씬으로 이동합니다");
        }
    }
}
