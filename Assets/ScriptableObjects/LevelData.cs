using UnityEngine;

/// <summary>
/// Data class to define a level data.
/// </summary>
[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    [SerializeField] private CellIndex[] brickIndices;
    [SerializeField] private EnemyData[] enemies;
    [SerializeField] private CellIndex[] powerUpIndices;
    [SerializeField] private CellIndex doorPosition;

    public CellIndex[] BrickIndices => brickIndices;

    public EnemyData[] Enemies => enemies;

    public CellIndex[] PowerUpIndices => powerUpIndices;

    public CellIndex DoorPosition => doorPosition;

    [System.Serializable]
    public struct EnemyData
    {
        [SerializeField] private EnemyProperties enemyProperties;
        [SerializeField] private CellIndex[] wayPoints;

        public EnemyProperties EnemyProperties => enemyProperties;

        public CellIndex[] WayPoints => wayPoints;
    }
}