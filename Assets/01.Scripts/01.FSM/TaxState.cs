using UnityEngine;

public class TaxState : IState
{
    public void Enter(GameManager gm)
    {
        Debug.Log(" [세금 납부] 세금을 징수합니다...");

        int dailyTax = 50; // (나중에 공식 적용: 100 * day...)

        if (gm.gold >= dailyTax)
        {
            gm.gold -= dailyTax;
            Debug.Log($"세금 {dailyTax}G 납부 완료! 남은 돈: {gm.gold}");

            // 납부 성공 시 -> 다음 날 아침이 밝습니다.
            gm.ChangeState(new DayState());
        }
        else
        {
            Debug.Log("돈이 부족합니다! 체력(Life)이 감소합니다.");
            // gm.DecreaseLife(1);

            // 살아있다면 다음 날로, 죽었다면 게임 오버로
            gm.ChangeState(new DayState());
        }
    }

    public void Execute(GameManager gm) { }
    public void Exit(GameManager gm) { }
}