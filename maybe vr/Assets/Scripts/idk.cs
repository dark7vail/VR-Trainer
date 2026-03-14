using UnityEngine;

[CreateAssetMenu(fileName = "ScenarioData", menuName = "VR Trainer/Scenario")]
public class ScenarioData : ScriptableObject
{
    public string scenarioName;
    public string size;
    public string scoring;      // "Kills", "Accuracy"
    public string mode;         // "Auto", "Semi"
    public string sceneName;    // scene to load
    public bool isFavorite;
}