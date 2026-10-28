using UnityEngine;

public class EnemyFactory
{
    public Enemy CreateEnemy(EnemyConfig enemyConfiguration, GameObject enemyPrefab, Vector3 position)
    {
        GameObject instance = Object.Instantiate(enemyPrefab, position, Quaternion.identity);
        
        if (instance.TryGetComponent(out Enemy enemy) == false)
            throw new MissingComponentException($"На префабе врага отсутствует {nameof(Enemy)}");

        Health health = new Health(enemyConfiguration.MaxHealth);
        Mover mover = new Mover(enemyConfiguration.MoveSpeed);
        
        enemy.Initialize(health, mover, enemyConfiguration.Damage);

        return enemy;
    }
}