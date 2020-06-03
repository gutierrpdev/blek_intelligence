using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class WebSessionManager : MonoBehaviour
{
    public static WebSessionManager instance = null;

    #region session_data
    private const string gameName = "Blek";
    private int orderInSequence = 0;
    #endregion

    #region custom_events
    public const string BEGIN_DRAWING = "BEGIN_DRAWING";
    public const string BEGIN_LOOPING = "BEGIN_LOOPING";
    public const string BLACK_TOUCHED = "BLACK_TOUCHED";
    public const string RESTART_LEVEL = "RESTART_LEVEL";
    public const string FIRST_TOUCH = "FIRST_TOUCH";
    #endregion

    [DllImport("__Internal")]
    private static extern void LogGameEvent(string eventJSON);

    [DllImport("__Internal")]
    private static extern void GameOver();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Make sure session manager persists between scenes.
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            // always keep current session manager instead of the one in the scene.
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        EventManager.BlackTouched += BlackTouched;
        EventManager.Drawing += BeginDrawing;
        EventManager.Looping += BeginLooping;
        EventManager.LevelRestart += RestartLevel;
        EventManager.FirstTouch += FirstTouch;
        EventManager.TimeUp += ExperimentEnd;
    }

    #region event_methods

    public void LevelStart(int levelNumber)
    {
        WebEventParameter[] parameters =
            {new WebEventParameter(WebEventParameter.LEVEL_NUMBER, levelNumber.ToString())};
        LogEvent(WebEvent.LEVEL_START, parameters);
    }

    public void LevelEnd(int levelNumber)
    {
        WebEventParameter[] parameters =
            {new WebEventParameter(WebEventParameter.LEVEL_NUMBER, levelNumber.ToString())};
        LogEvent(WebEvent.LEVEL_END, parameters);
    }

    private void BeginDrawing()
    {
        LogEvent(BEGIN_DRAWING);
    }

    private void BeginLooping()
    {
        LogEvent(BEGIN_LOOPING);
    }

    private void BlackTouched()
    {
        LogEvent(BLACK_TOUCHED);
    }

    private void FirstTouch()
    {
        LogEvent(FIRST_TOUCH);
    }

    private void RestartLevel()
    {
        LogEvent(RESTART_LEVEL);
    }

    public void TutorialStart()
    {
        LogEvent(WebEvent.TUTORIAL_START);
    }

    public void TutorialEnd()
    {
        LogEvent(WebEvent.TUTORIAL_END);
    }

    public void ExperimentStart()
    {
        EventManager.CallSessionStart();
        LogEvent(WebEvent.EXPERIMENT_START);
    }

    public void ExperimentEnd()
    {
        EventManager.CallSessionEnd();
        LogEvent(WebEvent.EXPERIMENT_END);
        // notify browser about game end
        Debug.Log("Game Over");
        GameOver();
        Application.Quit();
    }

    #endregion

    private void LogEvent(string eventName, WebEventParameter[] parameters = null)
    {
        WebEvent webEvent = new WebEvent
        {
            name = eventName,
            parameters = parameters,
            timestamp = CurrentTimestamp(),
            gameName = gameName,
            orderInSequence = orderInSequence
        };
        orderInSequence++;
        LogGameEvent(JsonUtility.ToJson(webEvent));
    }
    private int CurrentTimestamp()
    {
        return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
    }

    private void OnDisable()
    {
        EventManager.BlackTouched -= BlackTouched;
        EventManager.Drawing -= BeginDrawing;
        EventManager.Looping -= BeginLooping;
        EventManager.LevelRestart -= RestartLevel;
    }
}
