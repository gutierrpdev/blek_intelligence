[System.Serializable]
public class LevelStats
{
    // player id.
    public string playerName = "";
    // level id.
    public int levelNumber = 0;
    // accumulated trials for given level. (total number of drawing phases)
    public int numberOfTrials = 0;
    // time util first screen touch.
    public float codeAndSolveTime = 0f;
    // total time used for coding/solving (addition of partial codeAndSolveTimes).
    public float totalCodeAndSolveTime = 0f;
    // total time spent drawing trajectories.
    public float totalDrawingTime = 0f;
    // whether level was completed.
    public bool completed = false;
}
