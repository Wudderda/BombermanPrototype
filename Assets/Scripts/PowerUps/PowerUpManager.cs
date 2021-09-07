using UnityEngine;

/// <summary>
/// Manages power up related operations.
/// </summary>
public abstract class PowerUpManager : MonoBehaviour
{ 
    public abstract void AddListener(PowerUpChangeListener listener);
    public abstract void RemoveListener(PowerUpChangeListener listener);
}
