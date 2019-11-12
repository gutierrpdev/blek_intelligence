using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    private int remainingBalls = 0;
    private LevelStats stats = new LevelStats();
    public float lastTimeChecked = 0;

    private void Start()
    {
        ResetCount();
        // set current level.
        stats.levelNumber = SceneManager.GetActiveScene().buildIndex;
        lastTimeChecked = Time.time;
    }

    public void BeginDrawing()
    {
        // initialize code and solve time for first trace.
        if (stats.codeAndSolveTime == 0f)
            stats.codeAndSolveTime = Time.time - lastTimeChecked;

        // add time spent thinking and/or observing to total.
        stats.totalCodeAndSolveTime += Time.time - lastTimeChecked;
        lastTimeChecked = Time.time;
    }

    public void StopDrawing()
    {
        // add time spent drawing current trace to total.
        stats.totalDrawingTime += Time.time - lastTimeChecked;
        lastTimeChecked = Time.time;
    }

    public void ResetCount()
    {
        // number of tries increases every time level must be reseted.
        stats.numberOfTrials++;
        // calculate number of balls in scene.
        remainingBalls = FindObjectsOfType<BallObject>().Length;
    }

    public void DecreaseBallCount()
    {
        remainingBalls--;
        if (remainingBalls == 0)
        {
            // winning condition: update times, set as completed and create file with data.
            stats.totalCodeAndSolveTime += Time.time - lastTimeChecked;
            stats.completed = true;
            SaveToString();
            FindObjectOfType<LevelChanger>().FadeToNextLevel();
        }
    }

    public void SaveToString()
    {
        System.IO.File.WriteAllText(
            "C:/Users/PGS 2/blek_intelligence/JSON" + "/playerName_" + stats.levelNumber + ".json",
            JsonUtility.ToJson(stats, true));
    }
}
