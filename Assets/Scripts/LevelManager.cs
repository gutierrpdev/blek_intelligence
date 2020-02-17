using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    private static LevelManager instance = null;

    private int remainingBalls = 0;
    // private LevelStats stats = new LevelStats();
    // public float lastTimeChecked = 0;

    // personal user id for current player.
    private string user_id = "test_user";
    // time at which manager was awoken.
    private float start_timestamp;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //InitFirebase();
            start_timestamp = Time.time;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
            instance.Start();
            return;
        }
    }

    // When the app starts, check to make sure that we have
    // the required dependencies to use Firebase, and if not,
    // add them if possible.
    /*private void InitFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                Parameter[] parameters = {
                    new Parameter(FirebaseAnalytics.ParameterValue, "test_user")
                };
                LogToFirebase(FirebaseAnalytics.EventLogin, parameters);
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }*/

    private void Start()
    {
        ResetCount();
        // set current level.
        // stats.levelNumber = SceneManager.GetActiveScene().buildIndex;

        // lastTimeChecked = Time.time;
    }


    public void BeginDrawing()
    {
        // log drawing event to firebase.
        //LogToFirebase("drawing");

        // initialize code and solve time for first trace.
        // if (stats.codeAndSolveTime == 0f)
        //     stats.codeAndSolveTime = Time.time - lastTimeChecked;

        // add time spent thinking and/or observing to total.
        // stats.totalCodeAndSolveTime += Time.time - lastTimeChecked;
        // lastTimeChecked = Time.time;
    }

    public void StopDrawing()
    {
        // log drawing event to firebase.
        //LogToFirebase("drawing_stopped");

        // add time spent drawing current trace to total.
        // stats.totalDrawingTime += Time.time - lastTimeChecked;
        // lastTimeChecked = Time.time;
    }

    public void Restart()
    {
        //LogToFirebase("restarting");
    }

    public void Black()
    {
        //LogToFirebase("black");
    }

    public void ResetCount()
    {
        // number of tries increases every time level must be reseted.
        // stats.numberOfTrials++;
        // calculate number of balls in scene.
        remainingBalls = FindObjectsOfType<BallObject>().Length;
    }

    public void DecreaseBallCount()
    {
        remainingBalls--;
        Debug.Log("Remaining balls = " + remainingBalls);
        if (remainingBalls == 0)
        {
            // winning condition: update times, set as completed and create file with data.
            // stats.totalCodeAndSolveTime += Time.time - lastTimeChecked;
            // stats.completed = true;

            // fade to next level.
            FindObjectOfType<LevelChanger>().FadeToNextLevel();
        }
    }

    public int RemainingBalls()
    {
        return remainingBalls;
    }

    // adds user identifier and timestamp to event before logging it to Firebase.
    /*private void LogToFirebase(string eventName, Parameter[] parameters = null)
    {
        int len = 2;
        if (parameters != null)
            len += parameters.Length;

        Parameter[] res = new Parameter[len];
        res[0] = new Parameter(FirebaseAnalytics.ParameterValue, user_id);
        res[1] = new Parameter(FirebaseAnalytics.ParameterValue, Time.time - start_timestamp);
        if (parameters != null)
            parameters.CopyTo(res, 2);

        Debug.Log("Logging event " + eventName);
        FirebaseAnalytics.LogEvent(eventName, res);
    }*/

    public void SaveToString()
    {
        /*System.IO.File.WriteAllText(
            "C:/Users/PGS 2/blek_intelligence/JSON" + "/playerName_" + stats.levelNumber + ".json",
            JsonUtility.ToJson(stats, true));*/
    }
}
