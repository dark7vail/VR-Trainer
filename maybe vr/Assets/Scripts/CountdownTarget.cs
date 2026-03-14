using System.Collections;
using UnityEngine;
using TMPro;

public class CountdownTarget : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI countdownText;

    void Awake()
    {
        // Hide countdown text at start
        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
        else
            Debug.LogWarning("CountdownTarget: countdownText not assigned!");
    }

    public void OnShot()
    {
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        // Hide the target
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        // Show countdown text
        countdownText.gameObject.SetActive(true);

        countdownText.text = "3";
        yield return new WaitForSeconds(1f);

        countdownText.text = "2";
        yield return new WaitForSeconds(1f);

        countdownText.text = "1";
        yield return new WaitForSeconds(1f);

        // Hide countdown text
        countdownText.gameObject.SetActive(false);

        // Show timer now that game is starting
        if (GameManager.Instance?.timerUI != null)
            GameManager.Instance.timerUI.Show();

        gameObject.SetActive(false);
        GameManager.Instance.StartGame();
    }
}