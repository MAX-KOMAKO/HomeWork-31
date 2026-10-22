using UnityEngine;

[CreateAssetMenu(menuName = "Configs/EnemyConfig", fileName = "EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [field: SerializeField] public float MoveSpeed { get; private set; } = 3f;
    [field: SerializeField] public int MaxHealth { get; private set; } = 30;
    [field: SerializeField] public float DirectionChangeInterval { get; private set; } = 1.5f;
    [field: SerializeField] public int Damage { get; private set; } = 10;

    private void OnValidate()
    {
        if (MoveSpeed < 0) MoveSpeed = 0;
        if (MaxHealth < 1) MaxHealth = 1;
        if (DirectionChangeInterval < 0.1f) DirectionChangeInterval = 0.1f;
        if (Damage < 1) Damage = 1;
    }
}