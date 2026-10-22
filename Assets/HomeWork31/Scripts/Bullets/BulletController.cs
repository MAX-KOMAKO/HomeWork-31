using UnityEngine;

public class BulletController : Controller
{
    private Entity<Bullet> _bulletEntity;
    private ControllersUpdateService _updateService;
    private float _lifeTime;
    private float _currentLifeTime;

    public BulletController(Entity<Bullet> bulletEntity, ControllersUpdateService updateService, float lifeTime)
    {
        _bulletEntity = bulletEntity ?? throw new System.ArgumentNullException(nameof(bulletEntity));
        _updateService = updateService ?? throw new System.ArgumentNullException(nameof(updateService));
        _lifeTime = lifeTime;
        _currentLifeTime = lifeTime;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        if (_bulletEntity.Logic.IsActive == false || _bulletEntity.Transform == null)
        {
            DestroyBullet();
            return;
        }

        _bulletEntity.Logic.Update(deltaTime, _bulletEntity.Transform);

        _currentLifeTime -= deltaTime;
        if (_currentLifeTime <= 0)
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        _bulletEntity.Logic.Deactivate();
        _updateService.Remove(this);
        
        if (_bulletEntity.Transform != null)
        {
            Object.Destroy(_bulletEntity.Transform.gameObject);
        }
    }
}