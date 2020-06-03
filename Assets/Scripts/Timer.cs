using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // time left for user to complete session.
    private float secondsLeft = 0f;
    // time text tag
    public Text timeText;

    void Start()
    {
        EventManager.SessionStart += StartTimer;
        EventManager.SessionEnd += StopTimer;
    }

    private void StartTimer()
    {
        Debug.Log("Start timer");
        GetComponent<CanvasGroup>().alpha = 1;
        secondsLeft = 600f;
    }

    private void StopTimer()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        secondsLeft = 0f;
    }


    // Update is called once per frame
    private void Update()
    {
        if (timeText && secondsLeft > 0)
        {
            secondsLeft -= Time.deltaTime;
            timeText.text = ((int)secondsLeft / 60).ToString("D2") +
                ":" + ((int)secondsLeft % 60).ToString("D2");
            if(secondsLeft <= 0)
            {
                EventManager.CallTimeUp();
                StopTimer();
            }
        }
    }

    private void OnDisable()
    {
        EventManager.SessionStart -= StartTimer;
        EventManager.SessionEnd -= StopTimer;
    }
}
