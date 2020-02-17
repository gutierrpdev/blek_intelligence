using UnityEngine;

public class LevelSelector : MonoBehaviour
{

    public LevelChanger changer;

    public void Select(int levelIndex)
    {
        changer.FadeToLevel(levelIndex);
    }
}
