using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int gold = 100;

    private IState currentState;

    public GameObject curUnitPrefab;
    public int curUnitCost;

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

    public void ChangeState(IState newstate)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newstate;

        if (currentState != null)
        {
            currentState.Enter();
        }
    }

    private void Start()
    {
        ChangeState(new DayState());
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }
    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"골드 획득! {amount} 현재 : {gold}G");
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
            return true;
        }
    }

    public void SelectUnit(GameObject prefab, int cost)
    {
        curUnitPrefab = prefab;
        curUnitCost = cost;

        Debug.Log($"선택됨: {prefab.name}, 가격: {cost}");
        Debug.Log("이제 타일을 클릭하면 설치됩니다.");
    }
    public void OnTileClicked(Tile tile)
    {
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

        TryUseGold(curUnitCost);

        Instantiate(curUnitPrefab, tile.transform.position, Quaternion.identity);

        tile.isOccupied = true;
        Debug.Log("유닛 배치 완료");

        curUnitPrefab = null;

    }

   


    
}
