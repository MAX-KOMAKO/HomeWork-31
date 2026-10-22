using System;
using UnityEngine;

public class Enemy : IDamageable, IDisposable
{
    public event Action<Enemy> Died;

    private Health _health;
    private Mover _mover;

    public bool IsAlive => _health.IsAlive;
    public Health Health => _health;

    public Enemy(Health health, Mover mover)
    {
        _health = health ?? throw new ArgumentNullException(nameof(health));
        _mover = mover ?? throw new ArgumentNullException(nameof(mover));

        _health.Died += OnDied;
    }

    public void TakeDamage(int damage)
    {
        _health.TakeDamage(damage);
    }

    public void Move(Transform transform, Vector3 direction, float deltaTime)
    {
        if (IsAlive == false) return;
        _mover.Move(transform, direction, deltaTime);
    }

    public void Dispose()
    {
        _health.Died -= OnDied;
    }

    private void OnDied()
    {
        Died?.Invoke(this);
    }
}