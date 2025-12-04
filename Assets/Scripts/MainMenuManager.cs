using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string firstLevelSceneName = "Level1";
    public void StartGame()
    {
        Debug.Log("Memulai Game...");
        SceneManager.LoadScene(firstLevelSceneName);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Debug.Log("Keluar dari Game...");
        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
