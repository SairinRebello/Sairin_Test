using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private GameObject gameLosePanel;

    public static bool gameStart;
    public static bool gameOver;
    public static bool gameWin;
  
    public static Coordinates startPipeCordinates;
    public static Coordinates endCordinates;
    public static Coordinates endPipeCordinates;

    public Action onGameWinCheck;

    public int gameWinPoint;
    public int gameLosePoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void GameWin()
    {
        Time.timeScale = 0;
        gameWinPanel.SetActive(true);
    }

    public void GameFailed()
    {
        gameOver = true;
        Time.timeScale = 0;
        gameLosePanel.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene("Game Scene");
    }

    public void GameQuit()
    {
        Application.Quit();
    }


    public void WinCheck()
    {
        onGameWinCheck?.Invoke();
        Debug.Log("Win " +gameWinPoint + "\n Lose " + gameLosePoint);
        if(gameWinPoint > 3 && gameLosePoint< 1)
        {
            GameWin();
        }
        else
        {
            StartCoroutine(CheckWinAgain(.1f));
        }
    }

    public void RefreshGameWinValues()
    {
        gameWinPoint = gameLosePoint = 0;
    }

    private IEnumerator CheckWinAgain(float _waitTime)
    {
        yield return new WaitForSeconds(_waitTime);
        onGameWinCheck?.Invoke();
    }


}
