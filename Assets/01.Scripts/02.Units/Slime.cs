using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : UnitBase
{
    float timer;
    float incomeTime = 10f;
    int income = 5;

    void IncomeUp()
    {
        timer += Time.deltaTime;
        if (timer > incomeTime)
        {
            GameManager.Instance.gold += income;
            Debug.Log("°ñµå È¹µæ");

            timer = 0;
            
        }
    }

    protected override void Update()
    {

        IncomeUp();
    }
}