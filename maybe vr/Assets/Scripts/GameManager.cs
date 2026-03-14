using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Config")]
    public LevelConfig levelConfig;

    [Header("References")]
    public ResultsUI resultsUI;
    public CountdownTarget countdownTarget;
    public MovingTarget[] movingTargets;
    public TimerUI timerUI;

    // Stats
    public int Score { get; private set; }
    public int TotalShots { get; private set; }
    public int TotalHits { get; private set; }
    public float Accuracy => TotalShots == 0 ? 0f : (float)TotalHits / TotalShots * 100f;

    // State
    public bool GameActive { get; private set; } = false;
    private float timeRemaining;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        // Hide everything at start
        SetMovingTargetsActive(false);
        resultsUI?.Hide();
        timerUI?.Hide();

        // Show countdown target
        if (countdownTarget != null)
            countdownTarget.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!GameActive) return;

        timeRemaining -= Time.deltaTime;
        timerUI?.UpdateTimer(timeRemaining);

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            EndGame();
        }
    }

    public void StartGame()
    {
        Score = 0;
        TotalShots = 0;
        TotalHits = 0;
        timeRemaining = levelConfig.gameDuration;
        GameActive = true;

        SetMovingTargetsActive(true);
        timerUI?.UpdateTimer(timeRemaining);

        Debug.Log("[GameManager] Game Started!");
    }

    public void RegisterShot()
    {
        if (!GameActive) return;
        TotalShots++;
    }

    public void RegisterHit()
    {
        if (!GameActive) return;
        TotalHits++;
        Score += levelConfig.pointsPerHit;
        Debug.Log($"[GameManager] Hit! Score: {Score} | Accuracy: {Accuracy:F1}%");
    }

    void EndGame()
    {
        GameActive = false;
        SetMovingTargetsActive(false);
        timerUI?.Hide();

        if (resultsUI != null)
            resultsUI.Show(Score, Accuracy);
        else
            Debug.LogError("[GameManager] resultsUI is NULL!");

        Debug.Log($"[GameManager] Game Over! Score: {Score} | Accuracy: {Accuracy:F1}%");
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToLobby()
    {
        SceneManager.LoadScene(levelConfig.mainLobbyScene);
    }

    void SetMovingTargetsActive(bool active)
    {
        if (movingTargets == null) return;
        foreach (var t in movingTargets)
            if (t != null) t.gameObject.SetActive(active);
    }
}