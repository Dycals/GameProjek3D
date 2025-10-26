using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    public AudioSource audioSource;
    public AudioClip coinSFX;
    public AudioClip obstacleSFX;

    public GameObject gameOverCanvas; 
    public GameObject winCanvas;  

    private int score = 0;
    private bool isGameOver = false; 

    void Start()
    {
        UpdateScoreDisplay();


        if (gameOverCanvas != null) gameOverCanvas.SetActive(false);
        if (winCanvas != null) winCanvas.SetActive(false);


        Time.timeScale = 1f;
    }

   
    public bool IsGameOver()
    {
        return isGameOver;
    }

  
    void CheckGameCondition()
    {
        if (isGameOver) return; 

     
        if (score >= 100)
        {
            isGameOver = true;
            if (winCanvas != null) winCanvas.SetActive(true);
         
            Time.timeScale = 0f;
        }
       
        else if (score <= -5)
        {
            isGameOver = true;
            if (gameOverCanvas != null) gameOverCanvas.SetActive(true);
            
            Time.timeScale = 0f;
        }
    }

    public void CollectCoin()
    {
        if (isGameOver) return; 
        score += 10;
        UpdateScoreDisplay();

        if (audioSource != null && coinSFX != null)
        {
            audioSource.PlayOneShot(coinSFX);
        }

        CheckGameCondition(); 
    }

    public void HitObstacle()
    {
        if (isGameOver) return;

        score -= 5;
        UpdateScoreDisplay();

        if (audioSource != null && obstacleSFX != null)
        {
            audioSource.PlayOneShot(obstacleSFX);
        }

        CheckGameCondition();
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Skor: " + score;
        }
    }
}