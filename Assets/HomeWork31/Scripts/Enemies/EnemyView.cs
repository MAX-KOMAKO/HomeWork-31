using UnityEngine;

public class EnemyView : MonoBehaviour
{
    private Enemy _enemyLogic;
    private int _damage;

    public void Initialize(Enemy enemyLogic, int damage)
    {
        _enemyLogic = enemyLogic;
        _damage = damage;
    }

    public int GetDamage() => _damage;

    private void OnTriggerEnter(Collider other)
    {
        if (_enemyLogic == null) return;

        if (other.TryGetComponent(out BulletView bulletView))
        {
            if (bulletView.IsActive == false) return; // Защита от двойного урона

            _enemyLogic.TakeDamage(bulletView.GetDamage());
            bulletView.Deactivate();
            Debug.Log($"Враг получил урон! Осталось здоровья: {_enemyLogic.Health.CurrentHealth}");
        }
    }
}