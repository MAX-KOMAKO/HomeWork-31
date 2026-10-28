using System;

public class LoseByEnemyCount : IGameCondition
{
    public event Action Completed;

    private int _maxEnemiesAllowed;
    private ReactiveList<Enemy> _enemiesList;
    private bool _isEnabled;

    public LoseByEnemyCount(int maxEnemiesAllowed, ReactiveList<Enemy> enemiesList)
    {
        if (maxEnemiesAllowed <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxEnemiesAllowed), "Лимит врагов должен быть больше нуля.");

        _maxEnemiesAllowed = maxEnemiesAllowed;
        _enemiesList = enemiesList ?? throw new ArgumentNullException(nameof(enemiesList));
    }

    public void Enable()
    {
        _isEnabled = true;
        _enemiesList.Added += OnEnemyChanged;
        _enemiesList.Removed += OnEnemyChanged;
        CheckCondition();
    }

    public void Disable()
    {
        _isEnabled = false;
        _enemiesList.Added -= OnEnemyChanged;
        _enemiesList.Removed -= OnEnemyChanged;
    }

    public void Update(float deltaTime) { }

    private void OnEnemyChanged(Enemy enemy) => CheckCondition();

    private void CheckCondition()
    {
        if (_isEnabled && _enemiesList.Count > _maxEnemiesAllowed)
        {
            _isEnabled = false;
            Completed?.Invoke();
        }
    }
}