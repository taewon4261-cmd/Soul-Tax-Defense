using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public void Resume()
    {
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
    }
    
    public void RoadToMain()
    {
        Time.timeScale = 1f;
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }
        SceneManager.LoadScene("TitleScene");
    }

    public void Logout()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
