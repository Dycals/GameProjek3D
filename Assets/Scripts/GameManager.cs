using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    public static bool IsGameActive = true;

    public int ObjectivesCollected
    {
        get { return objectivesCollected; }
    }

    public int totalObjectives = 3;
    public int objectivesCollected = 0;

    public string currentLevelSceneName;
    public string nextLevelSceneName = "WinScene";

    private bool isGameOver = false;

    public GameObject gameOverPanel;
    public GameObject winPanel;

    void Start()
    {
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            IsGameActive = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        ObjectiveItem.OnObjectiveCollected += OnObjectiveCollected;
    }

    private void OnDisable()
    {
        ObjectiveItem.OnObjectiveCollected -= OnObjectiveCollected;
    }

    public void EndGame (bool won)
    {
        if (isGameOver) return;
        isGameOver = true;

        if (!IsGameActive) return;
        IsGameActive = false;

        Time.timeScale = 0f;

        if (won)
        {
            Debug.Log("Selamat! Anda telah menang.");
            winPanel.SetActive(true);
            //Invoke("LoadNextScene", 3f);
        }
        else
        {
            Debug.Log("Game Over. Tertangkap!");
            gameOverPanel.SetActive(true);
            //Invoke("RestartLevel", 3f);
        }
    }

    public void RestartLevelButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevelButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextLevelSceneName);
    }

    public void LoadMainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnObjectiveCollected(string objectiveName)
    {
        objectivesCollected++;
        Debug.Log($"Objective terkumpul: {objectivesCollected} / {totalObjectives}");

        if (objectivesCollected >= totalObjectives)
        {
            Debug.Log("Semua Objektif Terkumpul! Cari Jalan Keluar.");
        }
    }

    //private void RestartLevel()
    //{
    //    SceneManager.LoadScene(currentLevelSceneName);
    //}

    //private void LoadNextScene()
    //{
    //    SceneManager.LoadScene(nextLevelSceneName);
    //}

    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
