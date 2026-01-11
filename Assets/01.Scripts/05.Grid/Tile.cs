using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    private Color originalColor;
    private SpriteRenderer rend;

    public bool isOccupied;
    private UnitBase curUnit;

    private void Awake()
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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (isOccupied)
        {
            Debug.Log("이미 유닛이 있습니다");
            return;
        }
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnTileClicked(this);
        }
        rend.color = originalColor;
    }

    public void SetUnit(UnitBase unit)
    {
        isOccupied = true;
        curUnit = unit;
    }

    public void ClearUnit()
    {
        isOccupied = false;
        curUnit = null;
        Debug.Log("유닛이 죽어서 타일이 비었습니다");
    }

}