using System.Net.NetworkInformation;
using UnityEngine;

public class ContractState : IState
{
    public void Enter(GameManager gm)
    {
        Debug.Log(" [계약 단계] 악마와의 계약을 선택하세요.");

        // 1. 여기서 계약서 UI 팝업을 띄웁니다.
        // UIManager.Instance.ShowContractPopup(true);

        // (임시) 지금은 UI가 없으니 2초 뒤에 바로 세금 단계로 넘어가게 합시다.
        gm.StartCoroutine(TempTransition(gm));
    }

    public void Execute(GameManager gm) { } // 대기

    public void Exit(GameManager gm)
    {
        // UI 끄기
        // UIManager.Instance.ShowContractPopup(false);
    }

    // 임시 자동 넘김 코루틴 (나중에 버튼 클릭으로 교체하세요)
    System.Collections.IEnumerator TempTransition(GameManager gm)
    {
        yield return new WaitForSeconds(2f); // 2초 대기
        gm.ChangeState(new TaxState());      // 세금 단계로 이동
    }
}