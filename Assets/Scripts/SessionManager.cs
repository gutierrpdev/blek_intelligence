using UnityEngine;
using ServerEvents;
using System;

public class SessionManager : MonoBehaviour
{
    public static SessionManager instance = null;

    #region server_management
    private ServerEventManager eventManager;
    private const string url = "http://tfg.padaonegames.com/event";
    #endregion

    #region session_data
    private string userId;
    private const string gameName = "Blek";
    #endregion

    #region custom_events
    public const string BEGIN_DRAWING = "BEGIN_DRAWING";
    public const string BEGIN_LOOPING = "BEGIN_LOOPING";
    public const string BLACK_TOUCHED = "BLACK_TOUCHED";
    public const string RESTART_LEVEL = "RESTART_LEVEL";
    #endregion


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // initialize event manager using server's events endpoint.
            eventManager = new ServerEventManager(url);
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
    }

    // sets current user's id.
    public void SetUserId(string id)
    {
        userId = id;
    }

    #region event_methods

    public void LevelStart(int levelNumber)
    {
        ServerEventParameter[] parameters =
            {new ServerEventParameter(ServerEventParameter.LEVEL_NUMBER, levelNumber.ToString())};
        LogEvent(ServerEvent.LEVEL_START, parameters);
    }

    public void LevelEnd(int levelNumber)
    {
        ServerEventParameter[] parameters =
            {new ServerEventParameter(ServerEventParameter.LEVEL_NUMBER, levelNumber.ToString())};
        LogEvent(ServerEvent.LEVEL_END, parameters);
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

    private void RestartLevel()
    {
        LogEvent(RESTART_LEVEL);
    }

    public void TutorialStart()
    {
        LogEvent(ServerEvent.TUTORIAL_START);
    }

    public void TutorialEnd()
    {
        LogEvent(ServerEvent.TUTORIAL_END);
    }

    public void ExperimentStart()
    {
        EventManager.CallSessionStart();
        LogEvent(ServerEvent.EXPERIMENT_START);
    }

    public void ExperimentEnd()
    {
        EventManager.CallSessionEnd();
        LogEvent(ServerEvent.EXPERIMENT_END);
    }

    #endregion

    // fill event data with general information to be shared between all events logged from current session.
    private void LogEvent(string eventName, ServerEventParameter[] parameters = null)
    {
        ServerEvent gameEvent = new ServerEvent(userId, CurrentTimestamp(), gameName, eventName, parameters);
        eventManager.LogEvent(gameEvent);
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
