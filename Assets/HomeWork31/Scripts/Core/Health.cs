using System;

public class Health
{
    public event Action<int, int> HealthChanged;
    public event Action Died;

    private int _currentHealth;
    private int _maxHealth;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;
    public bool IsAlive => _currentHealth > 0;

    public Health(int maxHealth)
    {
        if (maxHealth <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxHealth), "Максимальное здоровье должно быть больше нуля.");

        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damage), "Урон не может быть отрицательным.");

        if (IsAlive == false) return;

        int oldHealth = _currentHealth;
        _currentHealth -= damage;
        
        if (_currentHealth < 0)
            _currentHealth = 0;

        HealthChanged?.Invoke(oldHealth, _currentHealth);

        if (IsAlive == false)
            Died?.Invoke();
    }
}