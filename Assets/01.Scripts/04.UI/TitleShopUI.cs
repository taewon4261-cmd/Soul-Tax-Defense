using UnityEngine;
using TMPro; 

public class TitleShopUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject shopPanel;
    public TextMeshProUGUI soulCountText; 

  
    public void OpenShop()
    {
        shopPanel.SetActive(true);
        RefreshUI();
    }

   
    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }

    private void RefreshUI()
    {
      
        if (DataManager.Instance != null)
        {
            soulCountText.text = $"{DataManager.Instance.SoulStones}";
        }
        else
        {
            soulCountText.text = "0";
            Debug.LogWarning("DataManager 인스턴스를 찾을 수 없습니다.");
        }
    }
}