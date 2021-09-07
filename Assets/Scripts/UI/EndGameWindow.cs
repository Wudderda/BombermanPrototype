using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Window to be displayed when the game is over.
/// </summary>
public class EndGameWindow : DefaultWindow
{
    [SerializeField] private Button playAgainButton;

    void Start()
    {
        playAgainButton.onClick.AddListener(() => GameManager.Instance.PlayAgain());
    }
}