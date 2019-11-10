using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public int remainingBalls = 0;

    private void Start()
    {
        ResetCount();
    }

    public void ResetCount()
    {
        remainingBalls = FindObjectsOfType<BallObject>().Length;
    }

    public void DecreaseBallCount()
    {
        remainingBalls--;
        if (remainingBalls == 0) NextLevel();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PreviousLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
