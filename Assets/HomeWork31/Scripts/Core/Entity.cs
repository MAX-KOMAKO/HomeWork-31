using UnityEngine;

public class Entity<T>
{
    public T Logic { get; }
    public Transform Transform { get; }

    public Entity(T logic, Transform transform)
    {
        Logic = logic;
        Transform = transform;
    }
}