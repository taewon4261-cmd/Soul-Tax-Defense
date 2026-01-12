using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    public TextMeshProUGUI rewardText;

    // GameManager나 WaveManager에서 승리 시 호출
    public void ShowResult(int rewardAmount)
    {
        this.gameObject.SetActive(true);
        rewardText.text = $"얻은 영혼석: {rewardAmount}";

        // 게임 정지 (필요 시)
        Time.timeScale = 0;
    }

    // 홈 버튼에 연결할 함수
    public void GoToTitle()
    {
        Time.timeScale = 1; // 정지 해제 필수
        SceneManager.LoadScene("TitleScene"); // 본인의 타이틀 씬 이름으로 변경
    }
}