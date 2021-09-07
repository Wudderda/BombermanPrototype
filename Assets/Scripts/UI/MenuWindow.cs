using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Window to be displayed when game is just started.
/// </summary>
public class MenuWindow : DefaultWindow
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    /// <summary>
    /// Starts the game.
    /// </summary>
    private void StartGame()
    {
        GameManager.Instance.LoadNextLevel();
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    private void QuitGame()
    {
        Application.Quit();
    }
}