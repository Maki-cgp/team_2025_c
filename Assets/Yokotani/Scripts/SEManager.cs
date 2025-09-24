using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance; 
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlaySE(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
