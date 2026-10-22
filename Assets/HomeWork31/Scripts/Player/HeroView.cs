using UnityEngine;

public class HeroView : MonoBehaviour
{
    private Hero _heroLogic;

    public void Initialize(Hero heroLogic)
    {
        _heroLogic = heroLogic;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_heroLogic == null) return;

        if (other.TryGetComponent(out EnemyView enemyView))
        {
            _heroLogic.TakeDamage(enemyView.GetDamage());
            Debug.Log($"Герой получил урон! Осталось здоровья: {_heroLogic.Health.CurrentHealth}");
        }
    }
}