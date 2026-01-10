using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayState : IState
{
    public void Enter(GameManager gm)
    {
        Debug.Log("낮 상태 진입! 유닛 배치시작");

        gm.isBattleActive = false;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowStartButton(true);
        }


    }
    public void Execute(GameManager gm)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.ChangeState(new NightState());
        }
    }
    public void Exit(GameManager gm)
    {
        Debug.Log("낮 상태 종료! 곧 밤이 됩니다.");
    }
}
