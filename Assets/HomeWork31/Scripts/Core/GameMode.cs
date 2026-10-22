using System;

public class GameMode : Controller
{
    public event Action Win;
    public event Action Defeat;

    private IGameCondition _winCondition;
    private IGameCondition _loseCondition;
    private bool _isGameEnded;

    public GameMode(IGameCondition winCondition, IGameCondition loseCondition)
    {
        _winCondition = winCondition ?? throw new ArgumentNullException(nameof(winCondition));
        _loseCondition = loseCondition ?? throw new ArgumentNullException(nameof(loseCondition));
    }

    public override void Enable()
    {
        base.Enable();
        _isGameEnded = false;

        _winCondition.Completed += OnWinConditionCompleted;
        _loseCondition.Completed += OnLoseConditionCompleted;

        _winCondition.Enable();
        _loseCondition.Enable();
    }

    public override void Disable()
    {
        base.Disable();

        _winCondition.Completed -= OnWinConditionCompleted;
        _loseCondition.Completed -= OnLoseConditionCompleted;

        _winCondition.Disable();
        _loseCondition.Disable();
    }

    protected override void UpdateLogic(float deltaTime)
    {
        if (_isGameEnded) return;

        _winCondition.Update(deltaTime);
        _loseCondition.Update(deltaTime);
    }

    private void OnWinConditionCompleted()
    {
        if (_isGameEnded) return;
        _isGameEnded = true;
        Win?.Invoke();
    }

    private void OnLoseConditionCompleted()
    {
        if (_isGameEnded) return;
        _isGameEnded = true;
        Defeat?.Invoke();
    }
}