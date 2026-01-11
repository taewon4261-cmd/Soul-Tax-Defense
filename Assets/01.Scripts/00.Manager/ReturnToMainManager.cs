using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainManager : MonoBehaviour
{
    public void ReturnToMain()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
