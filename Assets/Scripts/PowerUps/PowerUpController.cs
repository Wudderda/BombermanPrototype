using UnityEngine;

/// <summary>
/// Controller for power up operations.
/// </summary>
public class PowerUpController
{
    private const int maxBombRange = 5;
    private const int maxBombCount = 4;

    private int bombRange;
    public int BombRange => bombRange;

    private int bombCount;
    public int BombCount => bombCount;


    public PowerUpController(int bombRange, int bombCount)
    {
        this.bombRange = bombRange;
        this.bombCount = bombCount;
    }

    /// <summary>
    /// Increases given power for one level.
    /// </summary>
    /// <param name="powerUpType"></param>
    public void IncreasePowerLevel(PowerUpType powerUpType)
    {
        switch (powerUpType)
        {
            case PowerUpType.BOMB_RANGE:
                bombRange = Mathf.Clamp(bombRange + 1, 0, maxBombRange);
                break;
            case PowerUpType.BOMB_COUNT:
                bombCount = Mathf.Clamp(bombCount + 1, 0, maxBombCount);
                break;
        }
    }

    /// <summary>
    /// Resets the powers to the initial state.
    /// </summary>
    /// <param name="bombRange"></param>
    /// <param name="bombCount"></param>
    public void ResetValues(int bombRange, int bombCount)
    {
        this.bombRange = bombRange;
        this.bombCount = bombCount;
    }
}