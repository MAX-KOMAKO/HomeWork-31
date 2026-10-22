using UnityEngine;

public class Mover
{
    private const float MinDirectionSqrMagnitude = 0.01f;

    private float _speed;

    public Mover(float speed)
    {
        if (speed < 0)
            throw new System.ArgumentOutOfRangeException(nameof(speed), "Скорость не может быть отрицательной.");

        _speed = speed;
    }

    public void Move(Transform transform, Vector3 direction, float deltaTime)
    {
        if (direction.sqrMagnitude < MinDirectionSqrMagnitude) return;

        Vector3 normalizedDirection = direction.normalized;
        transform.position += normalizedDirection * _speed * deltaTime;
    }
}