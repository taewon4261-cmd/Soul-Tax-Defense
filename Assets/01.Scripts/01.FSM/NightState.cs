using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightState : IState
{
    public void Enter()
    {
        Debug.Log("밤 상태 진입! 적들이 몰려옵니다");
    }

    public void Execute()
    {
       //게임 오버 체크
    }

    public void Exit()
    {
        Debug.Log("밤 종료! 아침이 밝아옵니다.");
    }
}
