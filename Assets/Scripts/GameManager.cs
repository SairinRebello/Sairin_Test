using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static bool gameStarted;
    public static bool gamePaused;
    public static bool gameOver;

    public EventHandler OnGameOver;

    [SerializeField] private GameObject lasers;
    [SerializeField] private GameObject gamePausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWinPanel;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        OnGameOver += GameOver;
    }

    private void GameOver(object sender, EventArgs e)
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void GameStart()
    {
        gameStarted = true;
        lasers.SetActive(true);
    }

    public void GamePause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
            gamePausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            gamePausePanel.SetActive(false);
        }
    }

    public void GameWin()
    {
        Time.timeScale = 0;
        gameWinPanel.SetActive(true);
    }
    public void Replay()
    {
        SceneManager.LoadScene("Game Scene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void InitializeStart(float time)
    {
        Invoke("GameStart", time);
    }
}
