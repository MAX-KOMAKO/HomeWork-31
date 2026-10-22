using UnityEngine;

public class Bullet
{
    private Mover _mover;
    private Vector3 _direction;
    private int _damage;

    public bool IsActive { get; private set; }

    public Bullet(Mover mover, Vector3 direction, int damage)
    {
        _mover = mover;
        _direction = direction;
        _damage = damage;
        IsActive = true;
    }

    public void Update(float deltaTime, Transform transform)
    {
        if (IsActive == false) return;
        _mover.Move(transform, _direction, deltaTime);
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public int GetDamage() => _damage;
}