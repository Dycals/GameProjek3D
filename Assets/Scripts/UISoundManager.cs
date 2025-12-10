using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public static UISoundManager Instance { get; private set; }

    private AudioSource audioSource;

    public AudioClip buttonClickSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayClickSound()
    {
        if (buttonClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
        else
        {
            Debug.LogWarning("UISoundManager: AudioClip atau AudioSource hilang.");
        }
    }
}