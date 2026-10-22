using UnityEngine;

public enum WinConditionType
{
    SurviveTime,
    KillCount
}

public enum LoseConditionType
{
    PlayerDied,
    EnemyCountExceeded
}

[CreateAssetMenu(menuName = "Configs/LevelConfig", fileName = "LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [field: SerializeField] public WinConditionType WinType { get; private set; }
    [field: SerializeField] public LoseConditionType LoseType { get; private set; }
    
    [field: SerializeField] public float TimeToWin { get; private set; } = 30f;
    [field: SerializeField] public int KillsToWin { get; private set; } = 10;
    
    [field: SerializeField] public int MaxEnemiesAllowed { get; private set; } = 15;
    
    [field: SerializeField] public float SpawnInterval { get; private set; } = 2f;

    private void OnValidate()
    {
        if (TimeToWin < 0) TimeToWin = 0;
        if (KillsToWin < 1) KillsToWin = 1;
        if (MaxEnemiesAllowed < 1) MaxEnemiesAllowed = 1;
        if (SpawnInterval < 0.1f) SpawnInterval = 0.1f;
    }
}