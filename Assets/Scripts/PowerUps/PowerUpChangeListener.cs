using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Listener for power up changes.
/// </summary>
public interface PowerUpChangeListener
{
    /// <summary>
    /// Invoked when a power up is changed.
    /// </summary>
    /// <param name="powerUpController"></param>
    void PowerUpChanged(PowerUpController powerUpController);
}