using UnityEngine;
using UnityEngine.Video; // Wajib untuk memutar video
using UnityEngine.SceneManagement;

public class FinalVideoEnding : MonoBehaviour
{
    [Header("Video Settings")]
    public VideoPlayer videoPlayer; // Komponen Video Player
    public VideoClip goodEndingClip; // File video Good Ending
    public VideoClip badEndingClip;  // File video Bad Ending

    [Header("Game Settings")]
    public int reputationThreshold = 0; // Batas reputasi
    public string mainMenuSceneName = "MainMenu"; // Nama scene menu utama

    void Start()
    {
        int currentRep = 0;

        if (ReputationManager.Instance != null)
        {
            currentRep = ReputationManager.Instance.reputationScore;
        }

        if (currentRep >= reputationThreshold)
        {
            Debug.Log("Playing Good Ending Video");
            videoPlayer.clip = goodEndingClip;
        }
        else
        {
            Debug.Log("Playing Bad Ending Video");
            videoPlayer.clip = badEndingClip;
        }

        videoPlayer.loopPointReached += OnVideoFinished;

        videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video Selesai. Kembali ke Main Menu.");

        videoPlayer.loopPointReached -= OnVideoFinished;

        SceneManager.LoadScene(mainMenuSceneName);
    }
}