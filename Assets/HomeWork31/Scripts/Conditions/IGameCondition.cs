using System;

public interface IGameCondition
{
    event Action Completed;
    void Enable();
    void Update(float deltaTime);
    void Disable();
}