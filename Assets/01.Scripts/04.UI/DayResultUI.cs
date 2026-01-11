using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DayResultUI : MonoBehaviour
{
    [Header("UI 연결")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI incomeText;
    public TextMeshProUGUI taxText;
    public TextMeshProUGUI resultText; // 잔액 표시
    public Button nextDayButton;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.dayResultUI = this;
        }
        // 버튼 클릭 시 OnNextDayClicked 함수 실행
        if (nextDayButton != null)
            nextDayButton.onClick.AddListener(OnNextDayClicked);

        // 게임 시작 시 패널 꺼두기

        //this.gameObject.SetActive(false);
    }

    // TaxState에서 호출할 함수
    public void ShowResult()
    {
        GameManager gm = GameManager.Instance;

        Debug.Log("ShowResult 함수 진입 성공!");

        if (titleText == null) Debug.LogError("DayResultUI: Title Text가 연결 안 됨!");
        if (incomeText == null) Debug.LogError("DayResultUI: Income Text가 연결 안 됨!");
        if (taxText == null) Debug.LogError("DayResultUI: Tax Text가 연결 안 됨!");
        if (resultText == null) Debug.LogError("DayResultUI: Result Text가 연결 안 됨!");
        

        // 1. UI 텍스트 갱신
        titleText.text = $"{gm.day}일차 생존 성공!";
        incomeText.text = $"현재 보유: <color=#FFD700>{gm.gold:N0} G</color>"; // 노란색
        taxText.text = $"징수 세금: <color=#FF4500>-{gm.currentTax:N0} G</color>"; // 주황/빨강

        // 납부 후 예상 잔액 계산
        int futureGold = gm.gold - gm.currentTax;

        if (futureGold >= 0)
        {
            resultText.text = $"납부 후 잔액: <color=#00FF00>{futureGold:N0} G</color>";
        }
        else
        {
            // 돈 부족 -> 생명력 차감 경고
            int lifeDamage = Mathf.Abs(futureGold);
            resultText.text = $"<color=red>잔액 부족! 생명력 -{lifeDamage} 차감됩니다.</color>";
        }

        // 2. 패널 켜기 및 시간 정지
        this.gameObject.SetActive(true);
        Time.timeScale = 0;

        Debug.Log("패널 켜기 완료");
    }

    void OnNextDayClicked()
    {
        // 3. GameManager에게 "세금 내고 다음 날로 가라"고 명령
        GameManager.Instance.PayTaxAndAdvanceDay();

        Debug.Log("DayResultUI 종료");

        // 4. 패널 끄기 및 시간 재개
        this.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}