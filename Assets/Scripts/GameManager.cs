using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// Manager class for the bomberman game.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private List<LevelData> levels;

    private static GameManager instance;
    public static GameManager Instance => instance;

    private LevelConstructor levelConstructor;
    private List<LevelChangeListener> listeners;

    private int currentLevel = -1;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        listeners = new List<LevelChangeListener>();
        levelConstructor = new BombermanLevelConstructor();
    }

    public void AddListener(LevelChangeListener levelChangeListener)
    {
        listeners.Add(levelChangeListener);
    }

    public void RemoveListener(LevelChangeListener levelChangeListener)
    {
        listeners.Remove(levelChangeListener);
    }

    /// <summary>
    /// Starts the game from level 1.
    /// </summary>
    public void PlayAgain()
    {
        currentLevel = -1;
        LoadNextLevel();
    }

    /// <summary>
    /// Loads the next level.
    /// </summary>
    public void LoadNextLevel()
    {
        // Check for end level
        if (IsEndLevel())
        {
            GameCompleted();
            return;
        }

        StartCoroutine(LoadLevelRoutine(currentLevel + 1));
        currentLevel++;
    }

    private bool IsEndLevel() => currentLevel == levels.Count - 1;

    private void GameCompleted()
    {
        UIManager.Instance.ChangeWindow(UIManager.WindowType.GAME_COMPLETED);
    }

    private IEnumerator LoadLevelRoutine(int levelIndex)
    {
        Time.timeScale = 0;
        UIManager.Instance.StartLoading();
        yield return new WaitForSecondsRealtime(LoadingScreen.fadeInDuration);
        ObjectPoolManager.Instance.CollectAllLevelObjects();
        UIManager.Instance.ChangeWindow(UIManager.WindowType.GAME_WINDOW);
        levelConstructor.ConstructLevel(levels[levelIndex]);
        LevelUp(levels[levelIndex]);
        UIManager.Instance.StopLoading();
        Time.timeScale = 1;
    }

    private void LevelUp(LevelData newLevel)
    {
        foreach (LevelChangeListener levelChangeListener in listeners)
        {
            levelChangeListener.LevelChanged(newLevel);
        }
    }

    /// <summary>
    /// Opens the game over window.
    /// </summary>
    public void GameOver()
    {
        UIManager.Instance.ChangeWindow(UIManager.WindowType.GAME_OVER);
    }
}