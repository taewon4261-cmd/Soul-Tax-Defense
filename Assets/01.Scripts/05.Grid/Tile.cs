using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    private SpriteRenderer rend;

    public bool isOccupied;
    private UnitBase curUnit;

    private Color normalColor = new Color(0, 0, 0, 0.3f);
    private Color hoverColor = new Color(1, 0.92f, 0.016f, 0.5f);

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        // 시작할 때 색상을 normalColor로 강제 설정
        rend.color = normalColor;
    }

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && !isOccupied)
        {
            rend.color = hoverColor;
        }

    }

    private void OnMouseExit()
    {
        rend.color = normalColor;
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
        rend.color = normalColor;
    }

    public void SetUnit(UnitBase unit)
    {
        isOccupied = true;
        curUnit = unit;

        rend.enabled = false;
    }

    public void ClearUnit()
    {
        isOccupied = false;
        curUnit = null;

         rend.enabled = true;
        Debug.Log("유닛이 죽어서 타일이 비었습니다");
    }

}