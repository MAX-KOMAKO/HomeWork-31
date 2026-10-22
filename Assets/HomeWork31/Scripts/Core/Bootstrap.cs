using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    private const KeyCode RestartKey = KeyCode.R;

    [Header("Configs")]
    [SerializeField] private LevelConfig _levelConfiguration;
    [SerializeField] private HeroConfig _heroConfiguration;
    [SerializeField] private EnemyConfig _enemyConfiguration;

    [Header("Prefabs")]
    [SerializeField] private GameObject _heroPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _bulletPrefab;

    [Header("Scene")]
    [SerializeField] private Transform _heroSpawnPoint;
    [SerializeField] private Transform[] _enemySpawnPoints;

    private ControllersUpdateService _updateService;
    private HeroFactory _heroFactory;
    private EnemyFactory _enemyFactory;
    private BulletsFactory _bulletFactory;
    private EnemiesSpawner _enemiesSpawner;
    private GameMode _gameMode;
    private ReactiveList<Entity<Enemy>> _enemiesList;
    private Entity<Hero> _heroEntity;

    private void Awake()
    {
        ValidateDependencies();

        _updateService = new ControllersUpdateService();
        _heroFactory = new HeroFactory();
        _enemyFactory = new EnemyFactory();
        _bulletFactory = new BulletsFactory(_bulletPrefab);
        _enemiesList = new ReactiveList<Entity<Enemy>>();

        InitializeGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(RestartKey))
        {
            RestartGame();
        }

        _updateService?.Update(Time.deltaTime);
    }

    private void OnDestroy()
    {
        if (_enemiesSpawner != null)
            _enemiesSpawner.Spawned -= OnEnemySpawned;

        if (_gameMode != null)
        {
            _gameMode.Win -= OnGameWin;
            _gameMode.Defeat -= OnGameDefeat;
        }

        _heroEntity?.Logic.Dispose();
        _updateService?.Clear();
    }

    private void InitializeGame()
    {
        _heroEntity = _heroFactory.CreateHero(_heroConfiguration, _heroPrefab, _heroSpawnPoint.position);
        
        UnityInput input = new UnityInput();
        PlayerController playerController = new PlayerController(_heroEntity, input, _bulletFactory, _updateService, _heroConfiguration);
        _updateService.Add(playerController);

        _enemiesSpawner = new EnemiesSpawner(_enemyFactory, _enemyConfiguration, _enemyPrefab, _enemySpawnPoints, _levelConfiguration.SpawnInterval);
        _enemiesSpawner.Spawned += OnEnemySpawned;
        _updateService.Add(_enemiesSpawner);

        IGameCondition winCondition = CreateWinCondition();
        IGameCondition loseCondition = CreateLoseCondition(_heroEntity.Logic);

        _gameMode = new GameMode(winCondition, loseCondition);
        _gameMode.Win += OnGameWin;
        _gameMode.Defeat += OnGameDefeat;
        
        _updateService.Add(_gameMode);
    }

    private IGameCondition CreateWinCondition()
    {
        switch (_levelConfiguration.WinType)
        {
            case WinConditionType.SurviveTime:
                return new WinByTime(_levelConfiguration.TimeToWin);
            
            case WinConditionType.KillCount:
                return new WinByKills(_levelConfiguration.KillsToWin, _enemiesList);
            
            default:
                throw new System.ArgumentException($"Неизвестный тип условия победы: {_levelConfiguration.WinType}");
        }
    }

    private IGameCondition CreateLoseCondition(Hero hero)
    {
        switch (_levelConfiguration.LoseType)
        {
            case LoseConditionType.PlayerDied:
                return new LoseByDeath(hero);
            
            case LoseConditionType.EnemyCountExceeded:
                return new LoseByEnemyCount(_levelConfiguration.MaxEnemiesAllowed, _enemiesList);
            
            default:
                throw new System.ArgumentException($"Неизвестный тип условия поражения: {_levelConfiguration.LoseType}");
        }
    }

    private void OnEnemySpawned(Entity<Enemy> enemy)
    {
        _enemiesList.Add(enemy);
        
        EnemyRandomController enemyController = new EnemyRandomController(enemy, _updateService, _enemyConfiguration.DirectionChangeInterval);
        _updateService.Add(enemyController);

        enemy.Logic.Died += OnEnemyDied;
    }

    private void OnEnemyDied(Enemy enemy)
    {
        enemy.Died -= OnEnemyDied;

        for (int i = 0; i < _enemiesList.Count; i++)
        {
            if (_enemiesList.Elements[i].Logic == enemy)
            {
                _enemiesList.Remove(_enemiesList.Elements[i]);
                break;
            }
        }
    }

    private void OnGameWin()
    {
        Debug.Log("ПОБЕДА!");
        Time.timeScale = 0f;
    }

    private void OnGameDefeat()
    {
        Debug.Log("ПОРАЖЕНИЕ!");
        Time.timeScale = 0f;
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ValidateDependencies()
    {
        if (_levelConfiguration == null) Debug.LogError("LevelConfig не назначен!");
        if (_heroConfiguration == null) Debug.LogError("HeroConfig не назначен!");
        if (_enemyConfiguration == null) Debug.LogError("EnemyConfig не назначен!");
        if (_heroPrefab == null) Debug.LogError("HeroPrefab не назначен!");
        if (_enemyPrefab == null) Debug.LogError("EnemyPrefab не назначен!");
        if (_bulletPrefab == null) Debug.LogError("BulletPrefab не назначен!");
        if (_heroSpawnPoint == null) Debug.LogError("HeroSpawnPoint не назначен!");
        if (_enemySpawnPoints == null || _enemySpawnPoints.Length == 0) Debug.LogError("EnemySpawnPoints пуст!");
    }
}