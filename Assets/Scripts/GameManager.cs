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
    public GameObject objectivePanelUI;

    public float gameOverDelay = 1f;
    public float sceneSwitchDelay = 1f;

    [Header("Mobile Controls")]
    public GameObject joystickUI; 
    public GameObject interactButtonUI;

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
        //if (isGameOver) return;
        //isGameOver = true;

        //if (!IsGameActive) return;
        //IsGameActive = false;

        if (isGameOver || !IsGameActive) return;

        isGameOver = true;
        IsGameActive = false;

        StartCoroutine(ProcessGameOver(won));
    }

    IEnumerator ProcessGameOver(bool won)
    {
        yield return new WaitForSeconds(gameOverDelay);

        Time.timeScale = 0f;

        if (joystickUI != null) joystickUI.SetActive(false);
        if (interactButtonUI != null) interactButtonUI.SetActive(false);
        if (objectivePanelUI != null) objectivePanelUI.SetActive(false);

        if (won)
        {
            Debug.Log("Selamat! Anda telah menang.");
            winPanel.SetActive(true);
        }
        else
        {
            Debug.Log("Game Over. Tertangkap!");
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartLevelButton()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadSceneAfterDelay(currentSceneName));
    }

    public void LoadNextLevelButton()
    {
        StartCoroutine(LoadSceneAfterDelay(nextLevelSceneName));
    }

    public void LoadMainMenuButton()
    {
        StartCoroutine(LoadSceneAfterDelay("MainMenu"));
    }

    IEnumerator LoadSceneAfterDelay(string sceneName)
    {
        Time.timeScale = 1f;

        IsGameActive = true;
        Instance.isGameOver = false;

        yield return new WaitForSeconds(sceneSwitchDelay);

        SceneManager.LoadScene(sceneName);
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

    void Update()
    {
        
    }
}
