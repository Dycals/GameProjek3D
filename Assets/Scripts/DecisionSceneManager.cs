using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DecisionSceneManager : MonoBehaviour
{
    public string nextLevelSceneName;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Button sellButton;
    public Button courtButton;

    public int baseMoneyReward = 10;
    public int sellMoneyBonus = 5;

    public int sellReputationChange = -15;
    public int courtReputationChange = 10;

    // Start is called before the first frame update
    void Start()
    {
     if (ReputationManager.Instance == null)
        {
            Debug.LogError("ReputationManager tidak ditemukan. Pastikan sudah ada di scene awal dan DontDestroyOnLoad aktif.");
            return;
        }

        titleText.text = "Bukti Ditemukan!";
        descriptionText.text = "Anda memiliki bukti yang dapat dijual **(uang)** atau diserahkan ke pengadilan **(reputasi)**. Apa pilihan anda?";

        GiveBaseReward();

        sellButton.onClick.AddListener(() => MakeDecision(sellReputationChange, sellMoneyBonus));
        courtButton.onClick.AddListener(() => MakeDecision(courtReputationChange, 0));
    }

    private void GiveBaseReward()
    {
        if (ReputationManager.Instance != null)
        {
            ReputationManager.Instance.AdjustMoney(baseMoneyReward);
            Debug.Log($"Hadiah dasar {baseMoneyReward} Koin diterima.");
        }
    }

    public void MakeDecision(int reputationChange, int moneyBonus)
    {
        if (ReputationManager.Instance == null) return;

        ReputationManager.Instance.AdjustReputation(reputationChange);

        if (moneyBonus > 0)
        {
            ReputationManager.Instance.AdjustMoney(moneyBonus);
        }

        string outcome = (reputationChange > 0) ? "Keputusan yang bermoral." : "Keputusan yang korup: Uang tambahan didapat.";
        Debug.Log(outcome);

        SceneManager.LoadScene(nextLevelSceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
