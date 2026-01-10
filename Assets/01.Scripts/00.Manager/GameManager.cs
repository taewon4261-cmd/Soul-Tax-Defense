using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Data")]
    public int day = 1;        // 현재 날짜
    public int gold = 100;     // 현재 골드
    public int life = 5;       // 플레이어 생명
    public int currentTax = 50;// 다음 날 낼 세금

    public event Action OnResourceChange;

    private IState currentState;

    public GameObject curUnitPrefab;
    public int curUnitCost;

    public bool isBattleActive;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null) currentState.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    private void Start()
    {
        ChangeState(new DayState());
        OnResourceChange?.Invoke();
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.Execute(this);
        }
    }
    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"골드 획득! {amount} 현재 : {gold}G");

        OnResourceChange?.Invoke();
    }
    public bool TryUseGold(int amount)
    {
        if (gold < amount)
        {
            Debug.Log("돈이 부족합니다");
            return false;
        }
        else
        {
            gold -= amount;
            Debug.Log($"비용 : {amount}, 현재 잔액 : {gold} ");

            OnResourceChange?.Invoke();
            return true;
        }
    }

    // 날짜 넘길 때 호출할 함수 (나중에 TaxState 등에서 호출)
    public void NextDay()
    {
        day++;
        // 세금 증가 공식 (예: 하루 지날 때마다 50원씩 증가)
        currentTax += 50;

        OnResourceChange?.Invoke(); // 날짜/세금 바뀌었으니 UI 갱신
    }

    // 체력 감소 함수
    public void DecreaseLife(int amount)
    {
        life -= amount;
        if (life < 0) life = 0;

        OnResourceChange?.Invoke(); // 체력 바뀌었으니 UI 갱신

        if (life <= 0)
        {
            Debug.Log(" 게임 오버!");
            // ChangeState(new GameOverState()); // 나중에 추가
        }
    }
    public void SelectUnit(GameObject prefab, int cost)
    {
        curUnitPrefab = prefab;
        curUnitCost = cost;

        Debug.Log($"선택됨: {prefab.name}, 가격: {cost}");
        Debug.Log("이제 타일을 클릭하면 설치됩니다.");
    }

    // 버튼 클릭 이벤트용 함수 (UI 버튼에 연결하세요)
    public void OnClickStartBattle()
    {
        // 낮일 때만 밤으로 넘어갈 수 있음
        if (!isBattleActive)
        {
            ChangeState(new NightState());
        }
    }
    public void OnTileClicked(Tile tile)
    {
        if (isBattleActive)
        {
            Debug.Log("전투 중에는 유닛을 배치할 수 없습니다!");
            return;
        }
        if (curUnitPrefab == null)
        {
            Debug.Log("설치할 유닛을 먼저 선택하세요");
            return;
        }
        if (tile.isOccupied == true)
        {
            Debug.Log("이미 다른 유닛이 있습니다");
            return;
        }

        if (!TryUseGold(curUnitCost)) return;

        GameObject unitObj = Instantiate(curUnitPrefab, tile.transform.position, Quaternion.identity);

        UnitBase unit = unitObj.GetComponent<UnitBase>();
        if (unit != null)
        {
            unit.Setup(tile);
        }
        else
        {
            Debug.Log("생성된 프리팹에 UnitBase 스크립특라 없습니다");
        }
        Debug.Log("유닛 배치 완료");
        curUnitPrefab = null;
    }

   


    
}
