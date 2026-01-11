using UnityEngine;

public class TaxState : IState
{
    public void Enter(GameManager gm)
    {
        // 연결된 UI가 있는지 확인
        if (gm.dayResultUI != null)
        {
            // UI 스크립트 안에 panel.SetActive(true)가 들어있으므로
            // 이 함수만 호출하면 알아서 켜집니다.
            gm.dayResultUI.ShowResult();
        }
        else
        {
            // 만약 연결을 깜빡했다면 에러 로그 띄우고 자동 진행 (안전장치)
            Debug.LogError("GameManager에 DayResultUI가 연결되지 않았습니다! 인스펙터 확인하세요.");
            gm.PayTaxAndAdvanceDay();
        }
    }

    public void Execute(GameManager gm) { }
    public void Exit(GameManager gm) { }
}