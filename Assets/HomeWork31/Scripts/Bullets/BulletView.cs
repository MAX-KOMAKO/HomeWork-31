using UnityEngine;

public class BulletView : MonoBehaviour
{
    private Bullet _bulletLogic;
    private int _damage;

    public void Initialize(Bullet bulletLogic, int damage)
    {
        _bulletLogic = bulletLogic;
        _damage = damage;
    }

    public int GetDamage() => _damage;
    
    public bool IsActive => _bulletLogic != null && _bulletLogic.IsActive;

    public void Deactivate()
    {
        if (_bulletLogic != null)
            _bulletLogic.Deactivate();
    }
}