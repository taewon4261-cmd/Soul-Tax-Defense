using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
    public int life = 1000;
    public int maxLife = 1000;
    public int currentTax = 50;

    public event Action OnResourceChange;

    private IState currentState;
    public GameObject curUnitPrefab;
    public int curUnitCost;
    public bool isBattleActive;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        InitGameData(); // 데이터 초기화
        ChangeState(new DayState());
        OnResourceChange?.Invoke();
    }

    // 게임 데이터 및 버프 초기화 (재시작 시 필수)
    public void InitGameData()
    {
        day = 1;
        gold = 100;
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

    private void Update()
    {
        if (currentState != null) currentState.Execute(this);
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