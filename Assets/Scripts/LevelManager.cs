using UnityEngine;

/**
 * Maintains all relevant level information and triggers Level Completed events.
 */ 
public class LevelManager : MonoBehaviour
{
    // counts number of remaining balls to be eliminated in current level.
    private int remainingBalls = 0;

    // recalculate number of balls in scene.
    private void Start()
    {
        EventManager.LevelRestart += ResetCount;
        EventManager.BallTouched += DecreaseBallCount;
        ResetCount();
    }

    private void ResetCount()
    {
        // calculate number of balls in scene.
        remainingBalls = FindObjectsOfType<BallObject>().Length;
    }

    // decrease by one the number of active balls in scene.
    private void DecreaseBallCount()
    {
        remainingBalls--;
        // upon successfull removal of all active balls, notify about level completion.
        if (remainingBalls == 0)
        {
            EventManager.CallLevelCompleted();
        }
    }

    private void OnDisable()
    {
        EventManager.LevelRestart -= ResetCount;
        EventManager.BallTouched -= DecreaseBallCount;
    }
}
