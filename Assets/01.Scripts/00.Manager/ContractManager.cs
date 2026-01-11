using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq; // 리스트 섞기(Shuffle)를 위해 필요

public class ContractManager : MonoBehaviour
{
    public static ContractManager Instance;

    [Header("Data")]
    public List<ContractDataSO> allContracts; // 에디터에서 만든 SO 8개를 여기에 할당

    [Header("UI References")]
    public GameObject contractPanel;
    public Button[] cardButtons;
    public TextMeshProUGUI[] titleTexts;
    public TextMeshProUGUI[] descTexts;
    public Image[] iconImages; // 아이콘 표시용 (UI에 Image가 있다면 연결)

    [Header("Settings")]
    [Range(0f, 1f)] public float failChance = 0.25f; // 25% 꽝 확률

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // ContractState 진입 시 호출
    public void ShowRandomContracts()
    {
        contractPanel.SetActive(true);
        Time.timeScale = 0; // 시간 정지

        // 1. 리스트를 랜덤으로 섞어서 상위 3개 가져오기 (중복 방지)
        // OrderBy(GUID) 방식이 가장 간단한 셔플 방법 중 하나
        var selectedContracts = allContracts.OrderBy(x => System.Guid.NewGuid()).Take(3).ToList();

        // 2. UI 버튼 세팅
        for (int i = 0; i < cardButtons.Length; i++)
        {
            if (i < selectedContracts.Count)
            {
                ContractDataSO data = selectedContracts[i];
                cardButtons[i].gameObject.SetActive(true);

                // 텍스트/이미지 갱신
                titleTexts[i].text = data.contractName;
                descTexts[i].text = data.uiDescription; // data에 description 변수명 확인 필요
                if (iconImages != null && iconImages.Length > i && data.icon != null)
                {
                    iconImages[i].sprite = data.icon;
                }

                // 버튼 리스너 연결
                int index = i; // 클로저 캡처 방지
                cardButtons[i].onClick.RemoveAllListeners();
                cardButtons[i].onClick.AddListener(() => OnContractClicked(data));
            }
            else
            {
                cardButtons[i].gameObject.SetActive(false); // 데이터 없으면 버튼 끄기
            }
        }
    }

    // 버튼 클릭 시 호출
    void OnContractClicked(ContractDataSO data)
    {
        // 3. 25% 확률로 '꽝' 판정
        float roll = Random.value; // 0.0 ~ 1.0 랜덤

        if (roll < failChance)
        {
            // --- [꽝!] ---
            Debug.Log($"<color=red>[계약 실패] 사기꾼이었습니다! (확률: {roll:F2})</color>");

            // 패널티: 골드 10% 차감
            int penalty = Mathf.FloorToInt(GameManager.Instance.gold * 0.1f);
            GameManager.Instance.AddGold(-penalty);

            // TODO: 여기에 "사기당했습니다!" 팝업 연출을 넣으면 좋습니다.
        }
        else
        {
            // --- [성공!] ---
            Debug.Log($"<color=green>[계약 성공] {data.contractName} 적용. (확률: {roll:F2})</color>");

            // ScriptableObject의 효과 발동
            // (주의: data.effect가 Null이면 에러나니 에디터에서 꼭 연결 확인)
            if (data.effect != null)
            {
                data.effect.ApplyEffect();
            }
            else
            {
                Debug.LogError($"'{data.contractName}' 데이터에 Effect SO가 연결되지 않았습니다!");
            }
        }

        EndContractPhase();
    }

    void EndContractPhase()
    {
        contractPanel.SetActive(false);
        Time.timeScale = 1; // 시간 재개

        // 세금 납부 단계로 이동
        GameManager.Instance.ChangeState(new TaxState());
    }
}