using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void Enter(GameManager gm);

    public void Execute(GameManager gm);

    public void Exit(GameManager gm);
}