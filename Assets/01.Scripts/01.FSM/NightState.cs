using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightState : IState
{
    private WaveManager waveManager;
    public void Enter(GameManager gm)
    {
        Debug.Log("밤 상태 진입! 적들이 몰려옵니다");

        // 1. 유닛 배치 차단
        gm.isBattleActive = true;

        // 2. 현재 선택된 유닛이 있다면 취소 (손에 들고 있던 타워 내려놓기)
        gm.curUnitPrefab = null;

        // 3. 웨이브 시작 (WaveManager가 있다고 가정)
        // 만약 WaveManager를 싱글톤으로 만들었다면 아래처럼 호출
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.StartWave();
        }
    }

    public void Execute(GameManager gm)
    {
        if (WaveManager.Instance != null && WaveManager.Instance.IsWaveFinished())
        {
            // 웨이브가 끝났다면 다시 낮으로 전환 (혹은 결과/세금 단계로)
            gm.ChangeState(new DayState());
        }
    }

    public void Exit(GameManager gm)
    {
        Debug.Log("밤 종료! 아침이 밝아옵니다.");
    }
}
