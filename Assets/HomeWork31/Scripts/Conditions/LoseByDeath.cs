using System;

public class LoseByDeath : IGameCondition
{
    public event Action Completed;

    private Hero _heroLogic;
    private bool _isEnabled;

    public LoseByDeath(Hero heroLogic)
    {
        _heroLogic = heroLogic ?? throw new ArgumentNullException(nameof(heroLogic));
    }

    public void Enable()
    {
        _isEnabled = true;
        _heroLogic.Died += OnHeroDied;
    }

    public void Disable()
    {
        _isEnabled = false;
        _heroLogic.Died -= OnHeroDied;
    }

    public void Update(float deltaTime) { }

    private void OnHeroDied(Hero hero)
    {
        if (_isEnabled == false) return;
        
        _isEnabled = false;
        Completed?.Invoke();
    }
}