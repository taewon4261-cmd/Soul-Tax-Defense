using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Color originalColor;
    private SpriteRenderer rend;

    public bool isOccupied;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        originalColor = rend.color;
    }

    private void OnMouseEnter()
    {
        if (!isOccupied)
        {
            rend.color = Color.yellow;
        }

    }

    private void OnMouseExit()
    {
        rend.color = originalColor;
    }

    private void OnMouseDown()
    {
        if (isOccupied)
        {
            Debug.Log("이미 유닛이 있습니다");
            return;
        }
        GameManager.Instance.OnTileClicked(this);
        rend.color = originalColor;
    }

}










// public Vector2Int gridPos;

//    public bool IsBlocked => occupiedUnit != null;
//    public UnitBase occupiedUnit;

//    public UnitDataSO unitData;
//    public GameObject uniPrefab;

//    private Color originalColor;
//    private SpriteRenderer rend;

//    public bool TryPlace(UnitBase unit)
//{
//    if (occupiedUnit != null) return false;

//    occupiedUnit = unit;
//    unit.CurrentTile = this;
//    unit.transform.position = transform.position;
//    return true;
//}

//public void Clear()
//{
//    occupiedUnit = null;
//}

//private void Start()
//{
//    rend = GetComponent<SpriteRenderer>();
//    originalColor = rend.color;
//}

//private void OnMouseEnter()
//{
//    if (!IsBlocked)
//    {
//        rend.color = Color.yellow;
//    }
//}

//private void OnMouseExit()
//{
//    rend.color = originalColor;
//}

//private void OnMouseDown()
//{
//    if (IsBlocked)
//    {
//        Debug.Log("이미 유닛이 있습니다");
//        return;
//    }

//    if (!GameManager.Instance.TryUseGold(unitData.cost)) return;

//    GameObject go = Instantiate(uniPrefab, transform.position, Quaternion.identity);

//    UnitBase unit = go.GetComponent<UnitBase>();
//    if (unit == null)
//    {
//        Debug.Log("유닛이 없습니다 Prefab 확인");
//        Destroy(go);
//        return;
//    }

//    if (!TryPlace(unit))
//    {
//        Destroy(go);
//        return;
//    }


//    rend.color = originalColor;
//}