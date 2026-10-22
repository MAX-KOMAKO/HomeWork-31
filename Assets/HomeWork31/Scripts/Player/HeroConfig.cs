using UnityEngine;

[CreateAssetMenu(menuName = "Configs/HeroConfig", fileName = "HeroConfig")]
public class HeroConfig : ScriptableObject
{
    [field: SerializeField] public float MoveSpeed { get; private set; } = 5f;
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    
    [field: SerializeField] public float BulletSpeed { get; private set; } = 15f;
    [field: SerializeField] public int BulletDamage { get; private set; } = 25;
    [field: SerializeField] public float BulletLifetime { get; private set; } = 2f;

    private void OnValidate()
    {
        if (MoveSpeed < 0) MoveSpeed = 0;
        if (MaxHealth < 1) MaxHealth = 1;
        if (BulletSpeed < 1) BulletSpeed = 1;
        if (BulletDamage < 1) BulletDamage = 1;
        if (BulletLifetime < 0.1f) BulletLifetime = 0.1f;
    }
}