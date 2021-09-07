
/// <summary>
/// Interface to define generic object pool operations.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ObjectPool<T>
{
    /// <summary>
    /// Gets an object from the pool.
    /// </summary>
    /// <returns></returns>
    T GetObject();

    /// <summary>
    /// Puts an object back to the pool.
    /// </summary>
    /// <param name="obj"></param>
    void PutObject(T obj);

    /// <summary>
    /// Check for emptiness of the pool.
    /// </summary>
    /// <returns></returns>
    bool IsEmpty();
}
