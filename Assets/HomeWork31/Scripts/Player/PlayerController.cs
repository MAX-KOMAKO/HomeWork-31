using UnityEngine;

public class PlayerController : Controller
{
    private const float MinDirectionSqrMagnitude = 0.01f;

    private Entity<Hero> _heroEntity;
    private IInput _inputProvider;
    private BulletsFactory _bulletFactory;
    private ControllersUpdateService _updateService;
    private HeroConfig _heroConfiguration;

    public PlayerController(
        Entity<Hero> heroEntity, 
        IInput inputProvider, 
        BulletsFactory bulletFactory, 
        ControllersUpdateService updateService, 
        HeroConfig heroConfiguration)
    {
        _heroEntity = heroEntity;
        _inputProvider = inputProvider;
        _bulletFactory = bulletFactory;
        _updateService = updateService;
        _heroConfiguration = heroConfiguration;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        if (_heroEntity.Logic.IsAlive == false) return;

        Vector3 moveDirection = _inputProvider.GetMoveDirection();
        if (moveDirection.sqrMagnitude > MinDirectionSqrMagnitude)
        {
            _heroEntity.Logic.Move(_heroEntity.Transform, moveDirection, deltaTime);
            _heroEntity.Transform.forward = moveDirection.normalized;
        }

        if (_inputProvider.IsShootPressed())
        {
            _heroEntity.Logic.Shoot(_heroEntity.Transform, _heroEntity.Transform.forward, OnShoot);
        }
    }

    private void OnShoot(Vector3 position, Vector3 direction)
    {
        Entity<Bullet> bullet = _bulletFactory.CreateBullet(
            position, 
            direction, 
            _heroConfiguration.BulletSpeed, 
            _heroConfiguration.BulletDamage);
            
        BulletController bulletController = new BulletController(bullet, _updateService, _heroConfiguration.BulletLifetime);
        _updateService.Add(bulletController);
    }
}