using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IDisposable
{
    public event Action<Enemy> Died;

    private Health _health;
    private Mover _mover;

    public bool IsAlive => _health.IsAlive;
    public Health Health => _health;
    public int Damage { get; private set; }

    public void Initialize(Health health, Mover mover, int damage)
    {
        _health = health ?? throw new ArgumentNullException(nameof(health));
        _mover = mover ?? throw new ArgumentNullException(nameof(mover));
        Damage = damage;

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

    public void Dispose()
    {
        _health.Died -= OnDied;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bullet bullet))
        {
            TakeDamage(bullet.Damage);
        }
    }

    private void OnDied()
    {
        Died?.Invoke(this);
    }
}