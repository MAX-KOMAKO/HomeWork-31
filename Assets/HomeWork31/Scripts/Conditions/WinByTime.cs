using System;
using UnityEngine;

public class WinByTime : IGameCondition
{
    public event Action Completed;

    private float _targetTime;
    private float _currentTime;
    private bool _isEnabled;

    public WinByTime(float targetTime)
    {
        if (targetTime <= 0)
            throw new ArgumentOutOfRangeException(nameof(targetTime), "Время для победы должно быть больше нуля.");
            
        _targetTime = targetTime;
    }

    public void Enable()
    {
        _currentTime = 0f;
        _isEnabled = true;
    }

    public void Update(float deltaTime)
    {
        if (_isEnabled == false) return;

        _currentTime += deltaTime;
        if (_currentTime >= _targetTime)
        {
            _isEnabled = false;
            Completed?.Invoke();
        }
    }

    public void Disable() => _isEnabled = false;
}