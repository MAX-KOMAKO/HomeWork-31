using System;
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
    private ReactiveList<Enemy> _enemiesList;
    private Hero _hero;

    private void Awake()
    {
        ValidateDependencies();

        _updateService = new ControllersUpdateService();
        _heroFactory = new HeroFactory();
        _enemyFactory = new EnemyFactory();
        _bulletFactory = new BulletsFactory(_bulletPrefab);
        _enemiesList = new ReactiveList<Enemy>();

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

        _hero?.Dispose();
        _updateService?.Clear();
    }

    private void InitializeGame()
    {
        _hero = _heroFactory.CreateHero(_heroConfiguration, _heroPrefab, _heroSpawnPoint.position);
        
        UnityInput input = new UnityInput();
        PlayerController playerController = new PlayerController(_hero, input, _bulletFactory, _updateService, _heroConfiguration);
        _updateService.Add(playerController);

        _enemiesSpawner = new EnemiesSpawner(_enemyFactory, _enemyConfiguration, _enemyPrefab, _enemySpawnPoints, _levelConfiguration.SpawnInterval);
        _enemiesSpawner.Spawned += OnEnemySpawned;
        _updateService.Add(_enemiesSpawner);

        GameConditionsFactory conditionsFactory = new GameConditionsFactory();
        IGameCondition winCondition = conditionsFactory.CreateWinCondition(_levelConfiguration, _enemiesList);
        IGameCondition loseCondition = conditionsFactory.CreateLoseCondition(_levelConfiguration, _hero, _enemiesList);

        _gameMode = new GameMode(winCondition, loseCondition);
        _gameMode.Win += OnGameWin;
        _gameMode.Defeat += OnGameDefeat;
        
        _updateService.Add(_gameMode);
    }

    private void OnEnemySpawned(Enemy enemy)
    {
        _enemiesList.Add(enemy);
        
        EnemyRandomController enemyController = new EnemyRandomController(enemy, _updateService, _enemyConfiguration.DirectionChangeInterval);
        _updateService.Add(enemyController);

        enemy.Died += OnEnemyDied;
    }

    private void OnEnemyDied(Enemy enemy)
    {
        enemy.Died -= OnEnemyDied;
        _enemiesList.Remove(enemy); 
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
        if (_levelConfiguration == null) throw new ArgumentNullException(nameof(_levelConfiguration));
        if (_heroConfiguration == null) throw new ArgumentNullException(nameof(_heroConfiguration));
        if (_enemyConfiguration == null) throw new ArgumentNullException(nameof(_enemyConfiguration));
        if (_heroPrefab == null) throw new ArgumentNullException(nameof(_heroPrefab));
        if (_enemyPrefab == null) throw new ArgumentNullException(nameof(_enemyPrefab));
        if (_bulletPrefab == null) throw new ArgumentNullException(nameof(_bulletPrefab));
        if (_heroSpawnPoint == null) throw new ArgumentNullException(nameof(_heroSpawnPoint));
        if (_enemySpawnPoints == null || _enemySpawnPoints.Length == 0) throw new ArgumentNullException(nameof(_enemySpawnPoints));
    }
}