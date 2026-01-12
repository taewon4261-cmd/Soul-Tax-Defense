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
        if (GameManager.Instance == null || !GameManager.Instance.isBattleActive)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer > incomeTime)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddGold(income);

                SoundManager.Instance.PlaySFX(data.attackSound);
            }
            timer = 0;
        }
        

        }

    protected override void Update()
    {
        base.Update();
        IncomeUp();
    }
}