using UnityEngine;

public class BulletController : Controller
{
    private Bullet _bullet;
    private ControllersUpdateService _updateService;
    private float _lifeTime;
    private float _currentLifeTime;

    public BulletController(Bullet bullet, ControllersUpdateService updateService, float lifeTime)
    {
        _bullet = bullet ?? throw new System.ArgumentNullException(nameof(bullet));
        _updateService = updateService ?? throw new System.ArgumentNullException(nameof(updateService));
        _lifeTime = lifeTime;
        _currentLifeTime = lifeTime;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        if (_bullet == null || _bullet.IsActive == false)
        {
            DestroyBullet();
            return;
        }

        _bullet.Move(deltaTime);

        _currentLifeTime -= deltaTime;
        if (_currentLifeTime <= 0)
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        _updateService.Remove(this);
        
        if (_bullet != null)
        {
            Object.Destroy(_bullet.gameObject);
        }
    }
}