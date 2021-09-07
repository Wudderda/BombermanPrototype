using UnityEngine;
using static UIManager;

/// <summary>
/// The default window.
/// </summary>
public class DefaultWindow : MonoBehaviour
{
    [SerializeField] private WindowType windowType;
    [SerializeField] private Canvas canvas;

    /// <summary>
    /// Gets the type of the window.
    /// </summary>
    public WindowType WindowType => windowType;

    /// <summary>
    /// Opens the window.
    /// </summary>
    public virtual void OpenWindow() => canvas.enabled = true;

    /// <summary>
    /// Closes the window.
    /// </summary>
    public virtual void CloseWindow() => canvas.enabled = false;
}