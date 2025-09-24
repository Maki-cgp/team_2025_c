using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    private bool timerIsRunning = false;
    private float timeElapsed = 0f;

    // タイマーを開始する
    public void StartGameTimer()
    {
        timerIsRunning = true;
        timeElapsed = 0f;
    }

    // タイマーを停止する
    public void StopGameTimer()
    {
        timerIsRunning = false;
    }

    void Start()
    {
        StartGameTimer();
    }
    void Update()
    {
        if (timerIsRunning)
        {
            timeElapsed += Time.deltaTime;
        }
    }

    public float GetTimeElapsed()
    {
        return timeElapsed;
    }
}