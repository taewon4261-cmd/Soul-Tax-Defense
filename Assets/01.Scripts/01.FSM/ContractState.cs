using System.Net.NetworkInformation;
using UnityEngine;

public class ContractState : IState
{
    public void Enter(GameManager gm)
    {
        Debug.Log(" [계약 단계] 악마와의 계약을 선택하세요.");

        // 매니저에게 "카드 보여줘" 요청
        if (ContractManager.Instance != null)
        {
            ContractManager.Instance.ShowRandomContracts();
        }
        else
        {
            // 매니저 없으면 그냥 패스 (에러 방지)
            gm.ChangeState(new TaxState());
        }
    }

    public void Execute(GameManager gm) { } // 대기

    public void Exit(GameManager gm)
    {
        // UI 끄기
        // UIManager.Instance.ShowContractPopup(false);
    }
}