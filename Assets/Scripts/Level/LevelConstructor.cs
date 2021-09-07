/// <summary>
/// Interface to define common level construction operations.
/// </summary>
public interface LevelConstructor
{
    /// <summary>
    /// Constructs a new level with the given data.
    /// </summary>
    /// <param name="levelData">The level data</param>
    void ConstructLevel(LevelData levelData);
}