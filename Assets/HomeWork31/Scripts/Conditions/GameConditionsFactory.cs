using System;

public class GameConditionsFactory
{
    public IGameCondition CreateWinCondition(LevelConfig config, ReactiveList<Enemy> enemiesList)
    {
        switch (config.WinType)
        {
            case WinConditionType.SurviveTime:
                return new WinByTime(config.TimeToWin);
            case WinConditionType.KillCount:
                return new WinByKills(config.KillsToWin, enemiesList);
            default:
                throw new ArgumentException($"Неизвестный тип условия победы: {config.WinType}");
        }
    }

    public IGameCondition CreateLoseCondition(LevelConfig config, Hero hero, ReactiveList<Enemy> enemiesList)
    {
        switch (config.LoseType)
        {
            case LoseConditionType.PlayerDied:
                return new LoseByDeath(hero);
            case LoseConditionType.EnemyCountExceeded:
                return new LoseByEnemyCount(config.MaxEnemiesAllowed, enemiesList);
            default:
                throw new ArgumentException($"Неизвестный тип условия поражения: {config.LoseType}");
        }
    }
}