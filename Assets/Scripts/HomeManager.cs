using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Game Scene");
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
