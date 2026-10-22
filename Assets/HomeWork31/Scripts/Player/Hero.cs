using System;
using UnityEngine;

public class Hero : IDamageable, IDisposable
{
    public event Action<Hero> Died;

    private Health _health;
    private Mover _mover;
    private Shooter _shooter;

    public bool IsAlive => _health.IsAlive;
    public Health Health => _health;

    public Hero(Health health, Mover mover, Shooter shooter)
    {
        _health = health ?? throw new ArgumentNullException(nameof(health));
        _mover = mover ?? throw new ArgumentNullException(nameof(mover));
        _shooter = shooter ?? throw new ArgumentNullException(nameof(shooter));

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

    public void Shoot(Transform transform, Vector3 aimDirection, Action<Vector3, Vector3> onShoot)
    {
        if (IsAlive == false) return;
        _shooter.Shoot(transform.position, aimDirection, onShoot);
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