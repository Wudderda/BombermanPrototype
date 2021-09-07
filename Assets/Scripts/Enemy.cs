using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using static LevelData;

/// <summary>
/// The enemy 
/// </summary>
public class Enemy : MonoBehaviour
{
    private const float ENEMY_HEIGHT = 0.5f;

    private EnemyData enemyData;
    private IEnumerator moveRoutine;

    public void Init(EnemyData enemyData)
    {
        this.enemyData = enemyData;
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        transform.position = ConvertToWorldPosition(enemyData.WayPoints[0]);
        moveRoutine = LoopPath(enemyData.WayPoints);
        StartCoroutine(moveRoutine);
    }

    /// <summary>
    /// Follows the given path in a loop.
    /// </summary>
    /// <param name="wayPoints"></param>
    /// <returns></returns>
    IEnumerator LoopPath(CellIndex[] wayPoints)
    {
        foreach (CellIndex cellIndex in wayPoints)
        {
            yield return Move(cellIndex);
        }

        yield return LoopPath(Enumerable.Reverse(wayPoints).ToArray());
    }

    /// <summary>
    /// Moves the enemy to the given position.
    /// </summary>
    /// <param name="cellIndex"></param>
    /// <returns></returns>
    IEnumerator Move(CellIndex cellIndex)
    {
        Vector3 destination = ConvertToWorldPosition(cellIndex);
        while (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, enemyData.EnemyProperties.Speed * Time.deltaTime);
            yield return null;
        }
    }

    /// <summary>
    /// Converts cell index to the world position for enemy.
    /// </summary>
    /// <param name="cellIndex"></param>
    /// <returns></returns>
    private Vector3 ConvertToWorldPosition(CellIndex cellIndex) =>
        MapUtils.GetWorldPosition(cellIndex) - (Vector2.up * ENEMY_HEIGHT);

    /// <summary>
    /// Kills the enemy.
    /// </summary>
    public void Die()
    {
        ObjectPoolManager.Instance.PutObject(ObjectPoolEnum.ENEMY, this);
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
        }
    }
}