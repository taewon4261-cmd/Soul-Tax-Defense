using System.Collections.Generic;
using UnityEngine;
using TMPro;      // TextMeshPro 사용 시 필수
using UnityEngine.UI;

public class ContractManager : MonoBehaviour
{
    public static ContractManager Instance;

    [Header("Data")]
    public List<ContractDataSO> allContracts; // 게임에 존재하는 모든 계약서 리스트

    [Header("UI")]
    public GameObject contractPanel;   // 전체 패널
    public Button[] cardButtons;       // 카드 버튼 3개
    public TextMeshProUGUI[] titleTexts; // 각 버튼의 제목 텍스트 3개
    public TextMeshProUGUI[] descTexts;  // 각 버튼의 설명 텍스트 3개

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // ContractState에서 호출할 함수
    public void ShowRandomContracts()
    {
        contractPanel.SetActive(true); // 패널 켜기

        // 버튼 3개에 랜덤 데이터 세팅
        for (int i = 0; i < cardButtons.Length; i++)
        {
            // 리스트에서 랜덤으로 하나 뽑기 (중복 방지 로직은 나중에 추가)
            int randIndex = Random.Range(0, allContracts.Count);
            ContractDataSO data = allContracts[randIndex];

            // UI 갱신
            titleTexts[i].text = data.contractName;
            descTexts[i].text = data.description;

            // 버튼 클릭 이벤트 동적 연결 (이전 리스너 제거 후 추가)
            int index = i; // 클로저 문제 방지용 로컬 변수
            cardButtons[i].onClick.RemoveAllListeners();
            Debug.Log($"버튼 {i}번에 이벤트 연결함");

            cardButtons[i].onClick.AddListener(() =>
            {
                Debug.Log("버튼 클릭됨!"); // 클릭했을 때 이 로그가 뜨는지 확인
                OnContractSelected(data);
            });
        }

    }

    void OnContractSelected(ContractDataSO data)
    {
        Debug.Log($"계약 선택됨: {data.contractName}");

        // 1. 효과 적용
        ApplyEffect(data);

        // 2. 패널 끄기
        contractPanel.SetActive(false);

        // 3. 다음 단계(세금)로 이동
        GameManager.Instance.ChangeState(new TaxState());
    }

    void ApplyEffect(ContractDataSO data)
    {
        switch (data.type)
        {
            case ContractType.GoldGain:
                GameManager.Instance.AddGold((int)data.value);
                break;

            case ContractType.UnitAttackBuff:
                // TODO: 나중에 UnitManager를 통해 모든 유닛 공격력 증가 구현
                Debug.Log("아직 공격력 버프 기능은 미구현입니다.");
                break;

            case ContractType.TaxReduction:
                // TODO: 세금 감소 로직
                break;
        }
    }
}