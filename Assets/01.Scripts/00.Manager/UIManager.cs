using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class UIManager : MonoBehaviour
{
   
    public static UIManager Instance;

  
    [Header("UI Objects")]
    public GameObject startBattleButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    public void ShowStartButton(bool isShow)
    {
        if (startBattleButton != null)
        {
            startBattleButton.SetActive(isShow);
        }
        else
        {
            Debug.LogWarning("UIManager에 전투 시작 버튼이 연결되지 않았습니다!");
        }
    }
}