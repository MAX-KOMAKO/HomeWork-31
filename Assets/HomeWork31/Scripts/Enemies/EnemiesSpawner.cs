using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesSpawner : Controller
{
    public event Action<Entity<Enemy>> Spawned;

    private EnemyFactory _enemyFactory;
    private EnemyConfig _enemyConfiguration;
    private GameObject _enemyPrefab;
    private Transform[] _spawnPoints;
    private float _spawnInterval;
    private float _currentTime;

    public EnemiesSpawner(
        EnemyFactory enemyFactory, 
        EnemyConfig enemyConfiguration, 
        GameObject enemyPrefab, 
        Transform[] spawnPoints, 
        float spawnInterval)
    {
        _enemyFactory = enemyFactory ?? throw new ArgumentNullException(nameof(enemyFactory));
        _enemyConfiguration = enemyConfiguration ?? throw new ArgumentNullException(nameof(enemyConfiguration));
        _enemyPrefab = enemyPrefab ?? throw new ArgumentNullException(nameof(enemyPrefab));
        _spawnPoints = spawnPoints ?? throw new ArgumentNullException(nameof(spawnPoints));
        
        if (spawnInterval <= 0)
            throw new ArgumentOutOfRangeException(nameof(spawnInterval), "Интервал спавна должен быть больше нуля.");

        _spawnInterval = spawnInterval;
        _currentTime = spawnInterval;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        _currentTime -= deltaTime;
        if (_currentTime <= 0)
        {
            Spawn();
            _currentTime = _spawnInterval;
        }
    }

    private void Spawn()
    {
        if (_spawnPoints.Length == 0) return;
        
        Transform point = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        Entity<Enemy> enemy = _enemyFactory.CreateEnemy(_enemyConfiguration, _enemyPrefab, point.position);
        
        Spawned?.Invoke(enemy);
    }
}