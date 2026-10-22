using UnityEngine;

public class BulletsFactory
{
    private GameObject _bulletPrefab;

    public BulletsFactory(GameObject bulletPrefab)
    {
        _bulletPrefab = bulletPrefab ?? throw new System.ArgumentNullException(nameof(bulletPrefab));
    }

    public Entity<Bullet> CreateBullet(Vector3 position, Vector3 direction, float speed, int damage)
    {
        GameObject instance = Object.Instantiate(_bulletPrefab, position, Quaternion.identity);
        
        Mover mover = new Mover(speed);
        Bullet bullet = new Bullet(mover, direction, damage);

        if (instance.TryGetComponent(out BulletView view) == false)
            throw new MissingComponentException($"На префабе пули отсутствует {nameof(BulletView)}");

        view.Initialize(bullet, damage);

        return new Entity<Bullet>(bullet, instance.transform);
    }
}