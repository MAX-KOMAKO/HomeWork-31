using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyRandomController : Controller
{
    private const float MinDirectionSqrMagnitude = 0.01f;
    private const float RandomDirectionMin = -1f;
    private const float RandomDirectionMax = 1f;

    private Enemy _enemy;
    private ControllersUpdateService _updateService;
    private float _directionChangeInterval;
    
    private float _currentTimer;
    private Vector3 _currentDirection;

    public EnemyRandomController(Enemy enemy, ControllersUpdateService updateService, float directionChangeInterval)
    {
        _enemy = enemy;
        _updateService = updateService;
        _directionChangeInterval = directionChangeInterval;
        
        _enemy.Died += OnEnemyDied;
        ChooseNewDirection();
    }

    protected override void UpdateLogic(float deltaTime)
    {
        if (_enemy == null || _enemy.IsAlive == false) return;

        _currentTimer -= deltaTime;
        if (_currentTimer <= 0)
        {
            ChooseNewDirection();
        }

        _enemy.Move(_currentDirection, deltaTime);
    }

    private void ChooseNewDirection()
    {
        float randomX = Random.Range(RandomDirectionMin, RandomDirectionMax);
        float randomZ = Random.Range(RandomDirectionMin, RandomDirectionMax);
        _currentDirection = new Vector3(randomX, 0, randomZ).normalized;

        if (_currentDirection.sqrMagnitude > MinDirectionSqrMagnitude)
        {
            _enemy.transform.forward = _currentDirection;
        }

        _currentTimer = _directionChangeInterval;
    }

    private void OnEnemyDied(Enemy enemy)
    {
        _enemy.Died -= OnEnemyDied;
        _enemy.Dispose();
        
        _updateService.Remove(this);
        
        if (_enemy != null)
        {
            Object.Destroy(_enemy.gameObject);
        }
    }
}