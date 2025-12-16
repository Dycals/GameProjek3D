using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveUIManager : MonoBehaviour
{
    [Header("UI References")]
    public List<TextMeshProUGUI> objectiveTexts;
    public TextMeshProUGUI escapeText;

    [Header("Settings")]
    public Color incompleteColor = Color.red;
    public Color completeColor = Color.green;

    private void Start()
    {
        foreach (var textUI in objectiveTexts)
        {
            textUI.color = incompleteColor;
        }

        if (escapeText != null)
        {
            escapeText.gameObject.SetActive(false);
            escapeText.color = incompleteColor;
        }
    }

    private void OnEnable()
    {
        ObjectiveItem.OnObjectiveCollected += HandleObjectiveCollected;
    }

    private void OnDisable()
    {
        ObjectiveItem.OnObjectiveCollected -= HandleObjectiveCollected;
    }

    private void HandleObjectiveCollected(string objectiveName)
    {
        foreach (var textUI in objectiveTexts)
        {
            if (textUI.text.ToLower() == objectiveName.ToLower())
            {
                textUI.color = completeColor;
                break;
            }
        }

        StartCoroutine(CheckObjectivesWithDelay());
    }

    IEnumerator CheckObjectivesWithDelay()
    {
        yield return new WaitForEndOfFrame();

        if (GameManager.Instance != null)
        {
            Debug.Log($"Cek UI: Terkumpul {GameManager.Instance.ObjectivesCollected} / {GameManager.Instance.totalObjectives}");

            if (GameManager.Instance.ObjectivesCollected >= GameManager.Instance.totalObjectives)
            {
                ShowEscapeObjective();
            }
        }
    }

    private void ShowEscapeObjective()
    {
        if (escapeText != null)
        {
            escapeText.gameObject.SetActive(true);
        }
    }
}