using UnityEngine;

public class EnemyFactory
{
    public Entity<Enemy> CreateEnemy(EnemyConfig enemyConfiguration, GameObject enemyPrefab, Vector3 position)
    {
        GameObject instance = Object.Instantiate(enemyPrefab, position, Quaternion.identity);
        
        Health health = new Health(enemyConfiguration.MaxHealth);
        Mover mover = new Mover(enemyConfiguration.MoveSpeed);
        Enemy enemy = new Enemy(health, mover);

        if (instance.TryGetComponent(out EnemyView view) == false)
            throw new MissingComponentException($"На префабе врага отсутствует {nameof(EnemyView)}");

        view.Initialize(enemy, enemyConfiguration.Damage);

        return new Entity<Enemy>(enemy, instance.transform);
    }
}