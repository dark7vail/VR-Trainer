using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "VR Trainer/Level Config")]
public class LevelConfig : ScriptableObject
{
    [Header("Level Info")]
    public string levelName = "MovingTargets";
    public string mainLobbyScene = "MainLobby";

    [Header("Game Settings")]
    public float gameDuration = 60f;
    public int totalMovingTargets = 7;

    [Header("Target Movement")]
    public float targetMoveSpeed = 2f;
    public float targetMoveDistance = 3f;

    [Header("Scoring")]
    public int pointsPerHit = 1;
}