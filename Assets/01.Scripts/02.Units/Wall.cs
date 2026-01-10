using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wall : MonoBehaviour
{
    public void TakeDamage(int dmg)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.DecreaseLife(dmg);
        }
        // 시각적 효과 (선택 사항)
        // 예: 성벽이 빨갛게 깜빡거리거나 흔들리는 연출은 여기서 함
        Debug.Log("성벽 피격! 본체 Life 차감 요청함.");
    }
}
