using System;
using UnityEngine;

public class Hero : MonoBehaviour, IDamageable, IDisposable
{
    public event Action<Hero> Died;

    private Health _health;
    private Mover _mover;
    private IShooter _shooter;

    public bool IsAlive => _health.IsAlive;
    public Health Health => _health;

    public void Initialize(Health health, Mover mover, IShooter shooter)
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

    public void Move(Vector3 direction, float deltaTime)
    {
        if (IsAlive == false) return;
        _mover.Move(transform, direction, deltaTime);
    }

    public void Shoot(Vector3 aimDirection, Action<Vector3, Vector3> onShoot)
    {
        if (IsAlive == false) return;
        _shooter.Shoot(transform.position, aimDirection, onShoot);
    }

    public void Dispose()
    {
        _health.Died -= OnDied;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            TakeDamage(enemy.Damage);
        }
    }

    private void OnDied()
    {
        Died?.Invoke(this);
    }
}