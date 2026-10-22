using System;

public class WinByKills : IGameCondition
{
    public event Action Completed;

    private int _targetKills;
    private int _currentKills;
    private bool _isEnabled;
    private ReactiveList<Entity<Enemy>> _enemiesList;

    public WinByKills(int targetKills, ReactiveList<Entity<Enemy>> enemiesList)
    {
        if (targetKills <= 0)
            throw new ArgumentOutOfRangeException(nameof(targetKills), "Целевое количество убийств должно быть больше нуля.");
        
        _targetKills = targetKills;
        _enemiesList = enemiesList ?? throw new ArgumentNullException(nameof(enemiesList));
    }

    public void Enable()
    {
        _currentKills = 0;
        _isEnabled = true;
        _enemiesList.Removed += OnEnemyRemoved;
    }

    public void Disable()
    {
        _isEnabled = false;
        _enemiesList.Removed -= OnEnemyRemoved;
    }

    public void Update(float deltaTime) { }

    private void OnEnemyRemoved(Entity<Enemy> enemy)
    {
        if (_isEnabled == false) return;
        
        if (enemy.Logic.IsAlive == false)
        {
            _currentKills++;
            CheckCondition();
        }
    }

    private void CheckCondition()
    {
        if (_currentKills >= _targetKills && _isEnabled)
        {
            _isEnabled = false;
            Completed?.Invoke();
        }
    }
}