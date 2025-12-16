using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class PrologueManager : MonoBehaviour
{
    [Header("Settings")]
    public VideoPlayer videoPlayer; 
    public string firstLevelName = "Level1";

    void Start()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.loopPointReached += OnPrologueFinished;

        videoPlayer.Play();
    }

    void OnPrologueFinished(VideoPlayer vp)
    {
        Debug.Log("Prolog Selesai. Masuk ke Level 1...");

        videoPlayer.loopPointReached -= OnPrologueFinished;

        SceneManager.LoadScene(firstLevelName);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            OnPrologueFinished(videoPlayer);
        }
    }
}