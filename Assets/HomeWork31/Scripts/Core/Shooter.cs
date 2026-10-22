using System;
using UnityEngine;

public class Shooter
{
    private const float MinDirectionSqrMagnitude = 0.01f;

    public void Shoot(Vector3 origin, Vector3 direction, Action<Vector3, Vector3> onShoot)
    {
        if (onShoot == null)
            throw new ArgumentNullException(nameof(onShoot), "Действие выстрела не может быть пустым.");

        if (direction.sqrMagnitude < MinDirectionSqrMagnitude)
            return;

        Vector3 normalizedDirection = direction.normalized;
        onShoot.Invoke(origin, normalizedDirection);
    }
}