using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("[SceneManagement] Scene name is empty!");
            return;
        }

        Debug.Log($"[SceneManagement] Loading: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int sceneIndex)
    {
        if (sceneIndex < 0 || sceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError($"[SceneManagement] Scene index {sceneIndex} out of range!");
            return;
        }

        Debug.Log($"[SceneManagement] Loading scene index: {sceneIndex}");
        SceneManager.LoadScene(sceneIndex);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainLobby()
    {
        LoadScene("MainLobby");
    }

    public void LoadNextScene()
    {
        int next = SceneManager.GetActiveScene().buildIndex + 1;

        if (next >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("[SceneManagement] No next scene — going to MainLobby");
            LoadMainLobby();
            return;
        }

        LoadScene(next);
    }

    public void LoadPreviousScene()
    {
        int prev = SceneManager.GetActiveScene().buildIndex - 1;

        if (prev < 0)
        {
            Debug.Log("[SceneManagement] No previous scene — going to MainLobby");
            LoadMainLobby();
            return;
        }

        LoadScene(prev);
    }

    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}