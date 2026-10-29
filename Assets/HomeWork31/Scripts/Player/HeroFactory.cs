using UnityEngine;

public class HeroFactory
{
    public Hero CreateHero(HeroConfig heroConfiguration, GameObject heroPrefab, Vector3 position)
    {
        GameObject instance = Object.Instantiate(heroPrefab, position, Quaternion.identity);

        if (instance.TryGetComponent(out Hero hero) == false)
            throw new MissingComponentException($"На префабе героя отсутствует {nameof(Hero)}");

        Health health = new Health(heroConfiguration.MaxHealth);
        Mover mover = new Mover(heroConfiguration.MoveSpeed);
        Shooter shooter = new Shooter();

        hero.Initialize(health, mover, shooter);

        return hero;
    }
}