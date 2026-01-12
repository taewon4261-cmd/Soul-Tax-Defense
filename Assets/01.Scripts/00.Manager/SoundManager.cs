using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource audioSource;

    [Header("UI & System SFX")]
    public AudioClip buttonClick;
    public AudioClip contractSelect;  
    public AudioClip stageClear;    
    public AudioClip clickFail;
    public AudioClip btnClick;

    [Header("Combat SFX")]
    public AudioClip skeletonAtk;   
    public AudioClip impAtk;      
    public AudioClip enemyAtk;      
    public AudioClip slimeProduce;  
    public AudioClip barricadeHit;  

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴 방지
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }

    public void PlayButtonSound()
    {
        if (buttonClick != null)
        {
            audioSource.PlayOneShot(buttonClick, 0.5f);
        }
    }

    public void PlayContractSelect() // 계약서 선택
    {
        PlaySFX(contractSelect, 0.7f);
    }

    public void PlayStageClear() // 스테이지 클리어 (필요시)
    {
        PlaySFX(stageClear, 1.0f);
    }

    public void PlayClickFail() // 구매 실패 등
    {
        PlaySFX(clickFail, 0.6f);
    }
}