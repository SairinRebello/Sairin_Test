using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private GameObject gameLosePanel;

    public static Coordinates startPipeCordinates;
    public static Coordinates endCordinates;
    public static Coordinates endPipeCordinates;
    public static Dictionary<Coordinates, GameObject> allGridsPosition = new Dictionary<Coordinates, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GameWin()
    {
        Time.timeScale = 0;
        gameLosePanel.SetActive(true);
    }

    public void GameFailed()
    {
        Time.timeScale = 0;
        gameLosePanel.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene("Home");
    }

    public void GameQuit()
    {
        Application.Quit();
    }


    public void WinCheck()
    {

    }
}
