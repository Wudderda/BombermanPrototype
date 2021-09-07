using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object pool for unity components.
/// </summary>
public class ComponentObjectPool : ObjectPool<Component>
{
    // The prefab to be pooled
    private readonly GameObject objectPrefab;

    // The parent of pooled objects
    private readonly Transform parent;

    // The available objects
    private List<Component> availableObjects;

    // The objects in use
    private List<Component> objectsInUse;

    // The local position of the pool.
    private Vector3 poolPosition = Vector3.zero;

    public ComponentObjectPool(GameObject objectPrefab, Transform parent)
    {
        this.objectPrefab = objectPrefab;
        this.parent = parent;
    }

    /// <summary>
    /// Initializes object pool.
    /// </summary>
    /// <param name="capacity">The capacity of the pool</param>
    public void InitPool<T>(int capacity) where T : Component
    {
        // Create lists
        this.availableObjects = new List<Component>(capacity);
        this.objectsInUse = new List<Component>(capacity);

        // Instantiate available object pools
        for (int i = 0; i < capacity; i++)
        {
            AddNewObject<T>();
        }
    }

    /// <summary>
    /// Adds a new object to the available object pool.
    /// </summary>
    private void AddNewObject<T>() where T : Component => PutObject(CreateNewObject<T>());

    /// <summary>
    /// Creates a prefab under the parent object.
    /// </summary>
    /// <typeparam name="T">The type of the pooled object</typeparam>
    /// <returns></returns>
    private Component CreateNewObject<T>() where T : Component =>
        GameObject.Instantiate(objectPrefab, parent).GetComponent<T>();

    /// <summary>
    /// Collects all items in use back to the pool.
    /// </summary>
    public void CollectAllItems()
    {
        for (int i = objectsInUse.Count - 1; i >= 0; i--)
        {
            PutObject(objectsInUse[i]);
        }
    }

    public Component GetObject()
    {
        Component availableObject = availableObjects.RemoveLast();
        objectsInUse.Add(availableObject);
        return availableObject;
    }

    public void PutObject(Component obj)
    {
        obj.transform.localPosition = poolPosition;
        objectsInUse.Remove(obj);
        availableObjects.Add(obj);
    }

    public bool IsEmpty() => availableObjects.Count == 0;
}