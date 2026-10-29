using UnityEngine;

public class PlayerController : Controller
{
    private const float MinDirectionSqrMagnitude = 0.01f;

    private Hero _hero;
    private IInput _inputProvider;
    private BulletsFactory _bulletFactory;
    private ControllersUpdateService _updateService;
    private HeroConfig _heroConfiguration;

    public PlayerController(
        Hero hero, 
        IInput inputProvider, 
        BulletsFactory bulletFactory, 
        ControllersUpdateService updateService, 
        HeroConfig heroConfiguration)
    {
        _hero = hero;
        _inputProvider = inputProvider;
        _bulletFactory = bulletFactory;
        _updateService = updateService;
        _heroConfiguration = heroConfiguration;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        if (_hero.IsAlive == false) return;

        Vector3 moveDirection = _inputProvider.GetMoveDirection();
        if (moveDirection.sqrMagnitude > MinDirectionSqrMagnitude)
        {
            _hero.Move(moveDirection, deltaTime);
            _hero.transform.forward = moveDirection.normalized;
        }

        if (_inputProvider.IsShootPressed())
        {
            _hero.Shoot(_hero.transform.forward, OnShoot);
        }
    }

    private void OnShoot(Vector3 position, Vector3 direction)
    {
        Bullet bullet = _bulletFactory.CreateBullet(
            position, 
            direction, 
            _heroConfiguration.BulletSpeed, 
            _heroConfiguration.BulletDamage);
            
        BulletController bulletController = new BulletController(bullet, _updateService, _heroConfiguration.BulletLifetime);
        _updateService.Add(bulletController);
    }
}