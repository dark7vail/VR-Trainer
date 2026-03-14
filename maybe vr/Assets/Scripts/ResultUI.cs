using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsUI : MonoBehaviour
{
    [Header("References")]
    public GameObject panel;
    public TextMeshProUGUI levelNameText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI totalShotsText;
    public TextMeshProUGUI totalHitsText;
    public Button playAgainButton;
    public Button returnButton;

    void Awake()
    {
        if (panel != null)
            panel.SetActive(false);
        else
            Debug.LogWarning("ResultsUI: Panel not assigned!");
    }

    void Start()
    {
        if (playAgainButton != null)
            playAgainButton.onClick.AddListener(() => GameManager.Instance.PlayAgain());
        else
            Debug.LogWarning("ResultsUI: PlayAgainButton not assigned!");

        if (returnButton != null)
            returnButton.onClick.AddListener(() => GameManager.Instance.ReturnToLobby());
        else
            Debug.LogWarning("ResultsUI: ReturnButton not assigned!");
    }

    public void Show(int score, float accuracy)
    {
        if (panel == null) { Debug.LogError("ResultsUI: Panel is null!"); return; }

        if (levelNameText != null) levelNameText.text = GameManager.Instance.levelConfig.levelName;
        if (scoreText != null) scoreText.text = $"Score: {score}";
        if (accuracyText != null) accuracyText.text = $"Accuracy: {accuracy:F1}%";
        if (totalShotsText != null) totalShotsText.text = $"Shots Fired: {GameManager.Instance.TotalShots}";
        if (totalHitsText != null) totalHitsText.text = $"Hits: {GameManager.Instance.TotalHits}";

        panel.SetActive(true);
        Debug.Log($"[ResultsUI] Shown — Score: {score} | Accuracy: {accuracy:F1}%");
    }

    public void Hide()
    {
        if (panel != null) panel.SetActive(false);
    }
}