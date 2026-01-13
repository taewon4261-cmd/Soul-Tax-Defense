using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("난이도(아직 미구현)")]
    public int difficultyMultiplier = 1;

    [Header("배속버튼")]
    public GameObject speed1;
    public GameObject speed2;
    public GameObject speed3;

    [Header("PausePanel")]
    public GameObject pausePanel;

    [Header("UI References")]
    public DayResultUI dayResultUI;
    public ResultUI resultUI;

    public GameObject dayResultPanel;

    [Header("Global Buffs (계약 효과)")]
    public float skeletonAtkMult = 1f;
    public float skeletonHpMult = 1f;
    public float impAtkMult = 1f;
    public float impSpdMult = 1f;
    public float wallReflectDamage = 0f; // 0.5f = 50% 반사

    [Header("Special Flags")]
    public bool isSlimeDoubleBuffActive = false; // 슬라임 생산 2배
    public bool isSoulHarvestActive = false;     // 영혼 수확
    public float taxMultiplier = 1f;             // 세금 배율 (기본 1.0)

    [Header("Game Data")]
    public int day = 1;
    public int gold = 100;
    public int life = 500;
    public int maxLife = 500;
    public int currentTax = 50;

    [Header("Tax Settings")]
    public int baseTax = 50;        // 1일차 기본 세금
    public int taxIncreaseAmount = 30; // 하루마다 오르는 고정 세금
    public float taxGrowthRate = 1.2f; // 하루마다 10%씩 복리로 오르는 배율 (후반 난이도용)

    public event Action OnResourceChange;

    private IState currentState;
    public GameObject curUnitPrefab;
    public int curUnitCost;
    public bool isBattleActive;

    public WaveManager waveManager;

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;

        if (dayResultUI != null) dayResultUI.gameObject.SetActive(false);
        if (resultUI != null) resultUI.gameObject.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);

        // 2. 게임 데이터만 순수하게 초기화
        InitGameData();

        // 3. 상태를 강제로 '낮'으로 설정하여 루프를 처음부터 시작하게 함
        isBattleActive = false;
        ChangeState(new DayState());

        // 4. 자원 UI 업데이트 알림
        OnResourceChange?.Invoke();
    }
    public void FinishGame(bool isClear)
    {
        // 1. 보상 계산 로직
        // 웨이브당 10개 * 난이도 + 클리어 시 100개
        int waveReward = waveManager.currentWaveIndex * 10 * difficultyMultiplier;
        int clearBonus = isClear ? 100 : 0;
        int totalReward = waveReward + clearBonus;

        // 2. DataManager에 영혼석 저장
        if (DataManager.Instance != null && totalReward > 0)
        {
            DataManager.Instance.AddSoulStones(totalReward);
        }

        // 3. 결과 UI 띄우기 (이 한 줄이 핵심!)
        if (resultUI != null)
        {
            // 아까 만든 ResultUI의 ShowResult 함수 호출
            resultUI.ShowResult(totalReward);
        }

        // 4. 게임 정지
        Time.timeScale = 0;
    }
        

    public void CalculateNextTax()
    {
        // 공식: (현재 세금 + 고정 증가분) * 복리 배율 * (계약서 배율)
        float rawTax = (currentTax + taxIncreaseAmount) * taxGrowthRate;

        // 계약서 효과(taxMultiplier) 적용
        currentTax = Mathf.FloorToInt(rawTax * taxMultiplier);

        currentTax = Mathf.Max(0, Mathf.FloorToInt(rawTax * taxMultiplier));

        // taxMultiplier는 일회성이므로 다시 1로 초기화해야 함 (중요!)
        // 만약 영구 감면 계약이라면 초기화 안 해도 됨. 기획에 따라 결정.
        // 여기서는 '다음 날 세금 감면'이므로 초기화함.
        taxMultiplier = 1.0f;
    }

    public void PayTaxAndAdvanceDay()
    {
        Debug.Log($"[세금 납부] {currentTax} 골드 징수 시도.");

        // 1. 골드 납부 시도
        if (gold >= currentTax)
        {
            gold -= currentTax;
            Debug.Log("세금 납부 완료.");
        }
        else
        {
            // 돈 부족 -> 몸으로 때우기
            int deficit = currentTax - gold;
            gold = 0;
            DecreaseLife(deficit);
            Debug.Log($"세금 부족! 생명력 {deficit} 차감됨.");
        }

        if (day >= 7)
        {
            // 7일차면 계약서 UI 강제로 찾아 끄기
            GameObject dayResult = GameObject.Find("DayResultPanel");
            if (dayResult != null) dayResult.SetActive(false);

            FinishGame(true);
            return;
        }

        // 2. 다음 날 세금 책정
        CalculateNextTax();

        // 3. 날짜 증가 및 상태 변경
        day++;
        OnResourceChange?.Invoke();

        ChangeState(new DayState()); // 다시 낮으로
    }

    public void TogglePause()
    {
        if (Time.timeScale > 0)
        {
            Time.timeScale = 0; // 멈춤
            Debug.Log("게임 일시정지");

            pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1; // 보통 속도 재개
            Debug.Log("게임 재개");
        }
    }

    // 배속 버튼용 (1x, 2x, 3x)
    public void SetGameSpeed(float speed)
    {

        Time.timeScale = speed;
        Debug.Log($"게임 속도 변경: x{speed}");
        speed2.SetActive(true);
        speed1.SetActive(false);
    }
    public void SetGameSpeed2(float speed)
    {
        Time.timeScale = speed;
        Debug.Log($"게임 속도 변경: x{speed}");
        speed3.SetActive(true);
        speed2.SetActive(false);
    }
    public void SetGameSpeed3(float speed)
    {
        Time.timeScale = speed;
        Debug.Log($"게임 속도 변경: x{speed}");
        speed1.SetActive(true);
        speed3.SetActive(false);
    }

    // 게임 데이터 및 버프 초기화 (재시작 시 필수)
    public void InitGameData()
    {
        day = 1;
        gold = 100000;
        life = maxLife;
        currentTax = 50;

        // 버프 초기화
        skeletonAtkMult = 1f;
        skeletonHpMult = 1f;
        impAtkMult = 1f;
        impSpdMult = 1f;
        wallReflectDamage = 0f;
        isSlimeDoubleBuffActive = false;
        isSoulHarvestActive = false;
        taxMultiplier = 1f;
    }

    public void RepayTaxPartial(int amount)
    {
        // 1. 갚을 세금이 없으면 리턴
        if (currentTax <= 0)
        {
            Debug.Log("갚을 세금이 없습니다.");
            return;
        }
        // 2. 가진 돈보다 더 많이 갚을 순 없음
        if (gold < amount)
        {
           
            // 돈이 부족하면 가진 돈 전부를 털어서 갚는다 (선택 사항)
            amount = gold;
          
            if (amount == 0) return; // 0원이면 무시
        }

        // 3. 자원 처리
        gold -= amount;
        currentTax -= amount;

        if (currentTax < 0) currentTax = 0;

        Debug.Log($"[중도 상환] {amount}G 납부 완료. 남은 세금: {currentTax}");

        // 4. UI 갱신 (자원 변동 이벤트 호출)
        OnResourceChange?.Invoke();
    }

    private void Update()
    {
        if (currentState != null) currentState.Execute(this);

        if (Input.GetKeyDown(KeyCode.Space)) // 빠른확인을 위한 테스트용 코드
        {
            EnemyBase[] enemies = GameObject.FindObjectsOfType<EnemyBase>();

            foreach (EnemyBase enemy in enemies)
            {
                enemy.TakeDamage(999);
            }
        }
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null) currentState.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    // --- 자원 관리 ---
    public void AddGold(int amount)
    {
        gold += amount;
        if (gold < 0) gold = 0; // 음수 방지
        Debug.Log($"[Gold] {amount} 변경 -> 현재: {gold}G");
        OnResourceChange?.Invoke();
    }

    // 체력 회복 (계약 효과용)
    public void HealLife(int amount)
    {
        life += amount;
        if (life > maxLife) life = maxLife;
        OnResourceChange?.Invoke();
        Debug.Log($"[Life] 회복 +{amount} -> 현재: {life}");
    }

    public bool TryUseGold(int amount)
    {
        if (gold < amount)
        {
            Debug.Log("돈이 부족합니다");
            return false;
        }
        gold -= amount;
        OnResourceChange?.Invoke();
        return true;
    }

    public void DecreaseLife(int amount)
    {
        life -= amount;
        if (life < 0) life = 0;
        OnResourceChange?.Invoke();

        Debug.Log($"체력 감소: -{amount}, 남은 Life: {life}");
        if (life <= 0) GameOver();
    }

    // --- 날짜 및 세금 ---
    public void NextDay()
    {
        day++;

        // 세금 증가 공식: (현재 세금 + 50) * 배율
        // 배율(taxMultiplier)이 0.8이면 세금이 20% 줄어든 상태로 계산됨
        int baseIncrease = 50;
        currentTax = Mathf.FloorToInt((currentTax + baseIncrease) * taxMultiplier);
        if (currentTax < 0) currentTax = 0;

        Debug.Log($"[Day {day}] 세금 갱신: {currentTax} (배율: {taxMultiplier})");
        OnResourceChange?.Invoke();
    }

    // 세금 배율 조정 (계약 효과용)
    public void ModifyNextTaxRate(float amount)
    {
        // amount가 -0.2면 taxMultiplier는 0.8이 됨 (20% 할인)
        // amount가 0.3이면 1.3이 됨 (30% 증가)
        taxMultiplier += amount;
    }

    // --- 유닛 버프 적용 (계약 효과용) ---
    // UnitBuffEffectSO에서 정의한 Enum을 파라미터로 받음
    public void ApplyUnitBuff(UnitBuffEffectSO.TargetUnit unit, UnitBuffEffectSO.StatType stat, float val)
    {
        if (unit == UnitBuffEffectSO.TargetUnit.Skeleton)
        {
            if (stat == UnitBuffEffectSO.StatType.Attack) skeletonAtkMult += val;
            if (stat == UnitBuffEffectSO.StatType.HP) skeletonHpMult += val;
        }
        else if (unit == UnitBuffEffectSO.TargetUnit.Imp)
        {
            if (stat == UnitBuffEffectSO.StatType.Attack) impAtkMult += val;
            if (stat == UnitBuffEffectSO.StatType.AtkSpeed) impSpdMult += val;
        }
        else if (unit == UnitBuffEffectSO.TargetUnit.Wall)
        {
            if (stat == UnitBuffEffectSO.StatType.DamageReflect) wallReflectDamage = val;
        }
    }

    // --- 전투 및 배치 ---
    public void GameOver()
    {
        Debug.Log("게임 오버!");
        SceneManager.LoadScene("ResultScene");
    }

    public void SelectUnit(GameObject prefab, int cost)
    {
        curUnitPrefab = prefab;
        curUnitCost = cost;
    }

    public void OnClickStartBattle()
    {
        speed1.SetActive(false);
        speed2.SetActive(true);
        speed3.SetActive(false);
        if (!isBattleActive) ChangeState(new NightState());

    }

    public void OnTileClicked(Tile tile)
    {
        if (isBattleActive || curUnitPrefab == null || tile.isOccupied) return;

        if (!TryUseGold(curUnitCost)) return;

        GameObject unitObj = Instantiate(curUnitPrefab, tile.transform.position, Quaternion.identity);
        UnitBase unit = unitObj.GetComponent<UnitBase>();

        if (unit != null) unit.Setup(tile);

        curUnitPrefab = null; // 배치 후 선택 해제
    }
}