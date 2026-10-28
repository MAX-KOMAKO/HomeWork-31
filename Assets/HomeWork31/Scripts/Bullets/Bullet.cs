using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Mover _mover;
    private Vector3 _direction;

    public int Damage { get; private set; }
    public bool IsActive { get; private set; }

    public void Initialize(Mover mover, Vector3 direction, int damage)
    {
        _mover = mover;
        _direction = direction;
        Damage = damage;
        IsActive = true;
    }

    public void Move(float deltaTime)
    {
        if (IsActive == false) return;
        _mover.Move(transform, _direction, deltaTime);
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsActive == false) return;

        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(Damage);
            Deactivate();
        }
        else if (other.TryGetComponent(out Hero _) == false)
        {
            Deactivate();
        }
    }
}