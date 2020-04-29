using System;

public class EventManager{

    public static event Action LevelRestart;
    public static event Action FirstTouch;
    public static event Action SessionStart;
    public static event Action SessionEnd;
    public static event Action Looping;
    public static event Action LevelCompleted;
    public static event Action TimeUp;
    public static event Action Drawing;
    public static event Action<BallObject> BallTouched;
    public static event Action BlackTouched;

    public static void CallLevelRestart()
    {
        LevelRestart?.Invoke();
    }

    public static void CallFirstTouch()
    {
        FirstTouch?.Invoke();
    }

    public static void CallSessionStart()
    {
        SessionStart?.Invoke();
    }

    public static void CallSessionEnd()
    {
        SessionEnd?.Invoke();
    }

    public static void CallLooping()
    {
        Looping?.Invoke();
    }

    public static void CallLevelCompleted()
    {
        LevelCompleted?.Invoke();
    }

    public static void CallTimeUp()
    {
        TimeUp?.Invoke();
    }

    public static void CallDrawing()
    {
        Drawing?.Invoke();
    }

    public static void CallBallTouched(BallObject ball)
    {
        BallTouched?.Invoke(ball);
    }

    public static void CallBlackTouched()
    {
        BlackTouched?.Invoke();
    }
}
