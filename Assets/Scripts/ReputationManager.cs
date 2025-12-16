using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ReputationManager : MonoBehaviour
{
    public static ReputationManager Instance {  get; private set; }

    private const string REPUTATION_KEY = "PlayerReputation";
    private const string MONEY_KEY = "PlayerMoney";

    public int reputationScore = 0;
    public int playerMoney = 0;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI reputationText;

    public int ReputationScore => reputationScore;
    public int PlayerMoney => playerMoney;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindAndAssignUI();
    }

    public void FindAndAssignUI()
    {
        GameObject moneyObj = GameObject.FindWithTag("MoneyTextTag");
        GameObject repObj = GameObject.FindWithTag("ReputationTextTag");

        if (moneyObj != null)
        {
            moneyText = moneyObj.GetComponent<TextMeshProUGUI>();
        }

        if (repObj != null)
        {
            reputationText = repObj.GetComponent<TextMeshProUGUI>();
        }

        UpdateUI();
    }

    public void AdjustReputation(int amount)
    {
        reputationScore += amount;
        reputationScore = Mathf.Clamp(reputationScore, -100, 100);
        SaveData();
        UpdateUI();
        Debug.Log($"Reputasi diperbarui: {reputationScore}");
    }

    public void AdjustMoney(int amount)
    {
        playerMoney += amount;
        playerMoney = Mathf.Max(0, playerMoney);
        SaveData();
        UpdateUI();
        Debug.Log("Uang diperbarui: {playerMoney} Koin");
    }

    public void UpdateUI()
    {
        if (moneyText != null)
        {
            moneyText.text = $"Koin : {playerMoney}";
        }
        if (reputationText != null)
        {
            reputationText.text = $"Reputasi: {reputationScore}";
        }
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(REPUTATION_KEY, reputationScore);
        PlayerPrefs.SetInt(MONEY_KEY, PlayerMoney);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        reputationScore = PlayerPrefs.GetInt(REPUTATION_KEY, 0);
        playerMoney = PlayerPrefs.GetInt(MONEY_KEY, 0);
    }

    public void ResetData()
    {
        reputationScore = 0;
        playerMoney = 0;
        SaveData();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
