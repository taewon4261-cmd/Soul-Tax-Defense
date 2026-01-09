using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayState : IState
{
    public void Enter()
    {
        Debug.Log("낮 상태 진입! 유닛 배치시작");
    }
    public void Execute()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.ChangeState(new NightState());
        }
    }
    public void Exit()
    {
        Debug.Log("낮 상태 종료! 곧 밤이 됩니다.");
    }
}
