using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to manage window transitions.
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private WindowType initialWindowType;
    [SerializeField] private LoadingScreen loadingScreen;

    private static UIManager instance;
    public static UIManager Instance => instance;

    private Dictionary<WindowType, DefaultWindow> windowMap;
    private DefaultWindow activeWindow;

    private void Awake()
    {
        // Check for initialization
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Get windows
        windowMap = new Dictionary<WindowType, DefaultWindow>();
        DefaultWindow[] windows = GetComponentsInChildren<DefaultWindow>();
        foreach (DefaultWindow window in windows)
        {
            windowMap.Add(window.WindowType, window);
        }

        // Open the initial window
        DefaultWindow initialWindow = windowMap[initialWindowType];
        initialWindow.OpenWindow();
        activeWindow = initialWindow;
    }

    /// <summary>
    /// Changes the current window to the given window.
    /// </summary>
    /// <param name="type"></param>
    public void ChangeWindow(WindowType type)
    {
        activeWindow.CloseWindow();
        windowMap[type].OpenWindow();
        activeWindow = windowMap[type];
    }

    public void StartLoading()
    {
        loadingScreen.OpenLoadingScreen();
    }

    public void StopLoading()
    {
        loadingScreen.CloseLoadingScreen();
    }

    /// <summary>
    /// Enumeration to identify window types.
    /// </summary>
    public enum WindowType
    {
        START,
        GAME_WINDOW,
        GAME_OVER,
        GAME_COMPLETED
    }
}