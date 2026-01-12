using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public int SoulStones { get; private set; }

    private const string KEY_SOULSTONE = "Save_SoulStones";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadData()
    {
        SoulStones = PlayerPrefs.GetInt(KEY_SOULSTONE, 0);
    }

    public void AddSoulStones(int amount)
    {
        SoulStones += amount;
        PlayerPrefs.SetInt(KEY_SOULSTONE, SoulStones);
        PlayerPrefs.Save();

        Debug.Log($"[DataManager] ¿µÈ¥¼® {amount} È¹µæ! ÇöÀç ÃÑ: {SoulStones}");
    }

    public bool SpendSoulStones(int amount)
    {
        if (SoulStones >= amount)
        {
            SoulStones -= amount;
            PlayerPrefs.SetInt(KEY_SOULSTONE, SoulStones);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }
}