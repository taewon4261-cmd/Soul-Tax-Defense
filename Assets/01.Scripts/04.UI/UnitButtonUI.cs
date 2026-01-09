using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitButtonUI : MonoBehaviour
{

    public GameObject unitPrefab;

    public int cost;

    public void SelectThisUnit()
    {
        GameManager.Instance.SelectUnit(unitPrefab, cost);
    }
}
