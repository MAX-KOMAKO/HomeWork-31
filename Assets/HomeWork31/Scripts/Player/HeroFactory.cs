using UnityEngine;

public class HeroFactory
{
    public Entity<Hero> CreateHero(HeroConfig heroConfiguration, GameObject heroPrefab, Vector3 position)
    {
        GameObject instance = Object.Instantiate(heroPrefab, position, Quaternion.identity);
        
        Health health = new Health(heroConfiguration.MaxHealth);
        Mover mover = new Mover(heroConfiguration.MoveSpeed);
        Shooter shooter = new Shooter();
        Hero hero = new Hero(health, mover, shooter);

        if (instance.TryGetComponent(out HeroView view) == false)
            throw new MissingComponentException($"На префабе героя отсутствует {nameof(HeroView)}");

        view.Initialize(hero);

        return new Entity<Hero>(hero, instance.transform);
    }
}