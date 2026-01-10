using UnityEngine;

public class TaxState : IState
{
    public void Enter(GameManager gm)
    {
        Debug.Log(" [세금 납부] 세금을 징수합니다...");
        
        if (gm.gold >= gm.currentTax)
        {
            gm.gold -= gm.currentTax;
            Debug.Log($"세금 {gm.currentTax}G 납부 완료! 남은 돈: {gm.gold}");

            gm.NextDay();

            // 납부 성공 시 -> 다음 날 아침이 밝습니다.
            gm.ChangeState(new DayState());
        }
        else
        {
            int shortage = gm.currentTax - gm.gold; // 부족한 금액 계산
            gm.gold = 0;
            Debug.Log($"돈이 {shortage}G 부족합니다! 영혼(Life)으로 대체 납부합니다.");
            gm.DecreaseLife(shortage);
            if (gm.life > 0)
            {
                gm.NextDay();
                gm.ChangeState(new DayState());
            }
        }
    }

    public void Execute(GameManager gm) { }
    public void Exit(GameManager gm) { }
}