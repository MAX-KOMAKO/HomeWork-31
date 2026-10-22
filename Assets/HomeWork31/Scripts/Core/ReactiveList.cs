using System;
using System.Collections.Generic;

public class ReactiveList<T>
{
    public event Action<T> Added;
    public event Action<T> Removed;

    private List<T> _elements = new List<T>();

    public IReadOnlyList<T> Elements => _elements;
    public int Count => _elements.Count;

    public void Add(T element)
    {
        _elements.Add(element);
        Added?.Invoke(element);
    }

    public void Remove(T element)
    {
        if (_elements.Remove(element))
        {
            Removed?.Invoke(element);
        }
    }

    public void Clear()
    {
        for (int i = _elements.Count - 1; i >= 0; i--)
        {
            T element = _elements[i];
            _elements.RemoveAt(i);
            Removed?.Invoke(element);
        }
    }
}