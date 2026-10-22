using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyRandomController : Controller
{
    private const float MinDirectionSqrMagnitude = 0.01f;
    private const float RandomDirectionMin = -1f;
    private const float RandomDirectionMax = 1f;

    private Entity<Enemy> _enemyEntity;
    private ControllersUpdateService _updateService;
    private float _directionChangeInterval;
    
    private float _currentTimer;
    private Vector3 _currentDirection;

    public EnemyRandomController(Entity<Enemy> enemyEntity, ControllersUpdateService updateService, float directionChangeInterval)
    {
        _enemyEntity = enemyEntity;
        _updateService = updateService;
        _directionChangeInterval = directionChangeInterval;
        
        _enemyEntity.Logic.Died += OnEnemyDied;
        ChooseNewDirection();
    }

    protected override void UpdateLogic(float deltaTime)
    {
        if (_enemyEntity.Logic.IsAlive == false || _enemyEntity.Transform == null) return;

        _currentTimer -= deltaTime;
        if (_currentTimer <= 0)
        {
            ChooseNewDirection();
        }

        _enemyEntity.Logic.Move(_enemyEntity.Transform, _currentDirection, deltaTime);
    }

    private void ChooseNewDirection()
    {
        float randomX = Random.Range(RandomDirectionMin, RandomDirectionMax);
        float randomZ = Random.Range(RandomDirectionMin, RandomDirectionMax);
        _currentDirection = new Vector3(randomX, 0, randomZ).normalized;

        if (_currentDirection.sqrMagnitude > MinDirectionSqrMagnitude)
        {
            _enemyEntity.Transform.forward = _currentDirection;
        }

        _currentTimer = _directionChangeInterval;
    }

    private void OnEnemyDied(Enemy enemy)
    {
        _enemyEntity.Logic.Died -= OnEnemyDied;
        _enemyEntity.Logic.Dispose();
        
        _updateService.Remove(this);
        
        if (_enemyEntity.Transform != null)
        {
            Object.Destroy(_enemyEntity.Transform.gameObject);
        }
    }
}