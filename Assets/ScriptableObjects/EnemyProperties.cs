using UnityEngine;

/// <summary>
/// Data class to define an enemy.
/// </summary>
[CreateAssetMenu(fileName = "EnemyProperties", menuName = "ScriptableObjects/EnemyProperties", order = 2)]
public class EnemyProperties : ScriptableObject
{
    [SerializeField] private int speed;

    public int Speed => speed;
}