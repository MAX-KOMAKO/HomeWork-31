using UnityEngine;

public interface IInput
{
    Vector3 GetMoveDirection();
    bool IsShootPressed();
    bool IsRestartPressed();
}