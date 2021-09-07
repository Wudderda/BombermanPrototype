using UnityEngine;
using static LevelData;
using Random = System.Random;

/// <summary>
/// Level constructor for Bomberman game.
/// </summary>
public class BombermanLevelConstructor : LevelConstructor
{
    private readonly ObjectPoolManager objectPoolManager;

    public BombermanLevelConstructor()
    {
        objectPoolManager = ObjectPoolManager.Instance;
    }

    public void ConstructLevel(LevelData levelData)
    {
        // Place map objects
        PlaceBricks(levelData.BrickIndices);
        PlacePowerUps(levelData.PowerUpIndices);
        PlaceEnemies(levelData.Enemies);
        PlaceDoor(levelData.DoorPosition);
    }

    private void PlaceBricks(CellIndex[] brickIndices)
    {
        foreach (CellIndex brickIndex in brickIndices)
        {
            objectPoolManager.GetObject<Transform>(ObjectPoolEnum.BRICK).position =
                MapUtils.GetWorldPosition(brickIndex);
        }
    }

    private void PlacePowerUps(CellIndex[] powerUpIndices)
    {
        foreach (CellIndex powerUpIndex in powerUpIndices)
        {
            if (UnityEngine.Random.Range(0, 100) > 50)
                objectPoolManager.GetObject<Transform>(ObjectPoolEnum.BOMB_COUNT_POWERUP).position =
                    MapUtils.GetWorldPosition(powerUpIndex);
            else
                objectPoolManager.GetObject<Transform>(ObjectPoolEnum.BOMB_RANGE_POWERUP).position =
                    MapUtils.GetWorldPosition(powerUpIndex);
        }
    }

    private void PlaceEnemies(LevelData.EnemyData[] enemies)
    {
        foreach (EnemyData enemyData in enemies)
        {
            var enemyObject = objectPoolManager.GetObject<Enemy>(ObjectPoolEnum.ENEMY);
            enemyObject.Init(enemyData);
        }
    }

    private void PlaceDoor(CellIndex doorPosition)
    {
        objectPoolManager.GetObject<Transform>(ObjectPoolEnum.DOOR).position = MapUtils.GetWorldPosition(doorPosition);
    }
}