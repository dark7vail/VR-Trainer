using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    void Awake()
    {
        // Timer hidden until game starts
        Hide();
    }

    public void UpdateTimer(float seconds)
    {
        int s = Mathf.CeilToInt(seconds);
        timerText.text = s.ToString();
        timerText.color = s <= 10 ? Color.red : Color.white;
    }

    public void Show()
    {
        if (timerText != null)
            timerText.gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (timerText != null)
            timerText.gameObject.SetActive(false);
    }
}