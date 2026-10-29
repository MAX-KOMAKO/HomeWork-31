using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayController : Controller
{
    private ControllersUpdateService _updateService;
    private IInput _input;
    private GameMode _gameMode;

    public GameplayController(ControllersUpdateService updateService, IInput input, GameMode gameMode)
    {
        _updateService = updateService;
        _input = input;
        _gameMode = gameMode;

        _gameMode.Win += OnGameWin;
        _gameMode.Defeat += OnGameDefeat;
    }

    public override void Enable()
    {
        base.Enable();
        _updateService.Add(_gameMode);
    }

    public override void Disable()
    {
        base.Disable();
        _gameMode.Win -= OnGameWin;
        _gameMode.Defeat -= OnGameDefeat;
        _gameMode.Disable();
    }

    protected override void UpdateLogic(float deltaTime)
    {
        if (_input.IsRestartPressed())
        {
            RestartGame();
            return;
        }

        _updateService.Update(deltaTime);
    }

    private void OnGameWin()
    {
        EndGame("ПОБЕДА!");
    }

    private void OnGameDefeat()
    {
        EndGame("ПОРАЖЕНИЕ!");
    }

    private void EndGame(string message)
    {
        Debug.Log(message);
        Time.timeScale = 0f;
        _gameMode.Disable();
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}