using UnityEngine;
using TMPro;

public class TopBarUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI taxText;

    private void Start()
    {
        // 1. 게임 시작하자마자 한 번 갱신
        UpdateUI();

        // 2. GameManager의 방송 채널 구독 (이벤트 연결)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnResourceChange += UpdateUI;
        }
    }

    private void OnDestroy()
    {
        // ★ 중요: 오브젝트가 사라질 때 구독 취소 (메모리 누수 방지)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnResourceChange -= UpdateUI;
        }
    }

    // 실제로 텍스트를 바꾸는 함수
    void UpdateUI()
    {
        if (GameManager.Instance == null) return;

        // 문자열 보간($)을 사용해 깔끔하게 표시
        dayText.text = $"Day {GameManager.Instance.day}";
        lifeText.text = $"{GameManager.Instance.life}";
        goldText.text = $"{GameManager.Instance.gold}";
        taxText.text = $"{GameManager.Instance.currentTax}";
    }
}