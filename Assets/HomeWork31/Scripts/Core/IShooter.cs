using System;
using UnityEngine;

public interface IShooter
{
    void Shoot(Vector3 direction, Action<Vector3, Vector3> onShoot);
}