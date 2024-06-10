using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    [SerializeField] private float timeRemaining = 60f; 
    [SerializeField] private  Text timerText;
    [SerializeField] private InputField inputField;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerText.text = "00:00";
                timerText.color = Color.red;
                GameManager.Instance.GameFailed();
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void ResetTime(float _time)
    {
        timeRemaining = _time;
    }


    public void ResetTimerValue()
    {
        float _time = float.Parse(inputField.text);
        timeRemaining = _time;
    }

    public void StartTimer()
    {
        Time.timeScale = 1;
        UpdateTimerDisplay();
    }

}

