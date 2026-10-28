using UnityEngine;

public class BulletsFactory
{
    private GameObject _bulletPrefab;

    public BulletsFactory(GameObject bulletPrefab)
    {
        _bulletPrefab = bulletPrefab ?? throw new System.ArgumentNullException(nameof(bulletPrefab));
    }

    public Bullet CreateBullet(Vector3 position, Vector3 direction, float speed, int damage)
    {
        GameObject instance = Object.Instantiate(_bulletPrefab, position, Quaternion.identity);
        
        if (instance.TryGetComponent(out Bullet bullet) == false)
            throw new MissingComponentException($"На префабе пули отсутствует {nameof(Bullet)}");

        Mover mover = new Mover(speed);
        bullet.Initialize(mover, direction, damage);

        return bullet;
    }
}