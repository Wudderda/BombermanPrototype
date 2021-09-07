using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager for object pool related operations.
/// </summary>
public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabList;

    private static ObjectPoolManager instance;
    public static ObjectPoolManager Instance => instance;

    private Dictionary<ObjectPoolEnum, ComponentObjectPool> objectPoolMap;

    void Awake()
    {
        // Check for initialization
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        InitPools();
    }

    private void InitPools()
    {
        objectPoolMap = new Dictionary<ObjectPoolEnum, ComponentObjectPool>();
        int index = 0;
        foreach (ObjectPoolEnum type in Enum.GetValues(typeof(ObjectPoolEnum)))
        {
            ComponentObjectPool objectPool =
                new ComponentObjectPool(prefabList[index], transform);
            InitPool(objectPool, type);
            objectPoolMap.Add(type, objectPool);
            index++;
        }
    }

    private void InitPool(ComponentObjectPool pool, ObjectPoolEnum objectEnum)
    {
        switch (objectEnum)
        {
            case ObjectPoolEnum.BOMB:
                pool.InitPool<Transform>((int) objectEnum);
                break;
            case ObjectPoolEnum.BRICK:
                pool.InitPool<Transform>((int) objectEnum);
                break;
            case ObjectPoolEnum.ENEMY:
                pool.InitPool<Enemy>((int) objectEnum);
                break;
            case ObjectPoolEnum.EXPLOSION_EFFECT:
                pool.InitPool<Animator>((int) objectEnum);
                break;
            case ObjectPoolEnum.BOMB_COUNT_POWERUP:
                pool.InitPool<Transform>((int) objectEnum);
                break;
            case ObjectPoolEnum.BOMB_RANGE_POWERUP:
                pool.InitPool<Transform>((int) objectEnum);
                break;
            case ObjectPoolEnum.DOOR:
                pool.InitPool<Transform>((int) objectEnum);
                break;
        }
    }

    /// <summary>
    /// Gets an object of given type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public T GetObject<T>(ObjectPoolEnum type) where T : Component =>
        (T) (objectPoolMap[type].GetObject());

    /// <summary>
    /// Puts an object of given type.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="objectToPool"></param>
    public void PutObject(ObjectPoolEnum type, Component objectToPool) => objectPoolMap[type].PutObject(objectToPool);

    /// <summary>
    /// Collects all pool objects in use.
    /// </summary>
    public void CollectAllLevelObjects()
    {
        foreach (var objectPoolPair in objectPoolMap)
        {
            if (objectPoolPair.Key != ObjectPoolEnum.BOMB && objectPoolPair.Key != ObjectPoolEnum.EXPLOSION_EFFECT)
            {
                objectPoolPair.Value.CollectAllItems();
            }
        }
    }
}