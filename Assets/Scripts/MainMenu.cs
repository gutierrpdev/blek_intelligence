using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public void StartTutorial()
    {
        Debug.Log("Tutorial Level Set selected.");
        // load tutorial level set, if possible, and start level 0 of this set.
        LevelChanger.instance.FadeToLevelSet(LevelChanger.TUTORIAL_LEVEL);
    }
    
    public void StartSession()
    {
        Debug.Log("Session Level Set selected.");
        // load session level set, if possible, and start level 0 of this set.
        LevelChanger.instance.FadeToLevelSet(LevelChanger.SESSION_LEVEL);
    }

    public void ChangeUser()
    {
        Debug.Log("Change User selected.");
        // load user change screen, if possible.
        LevelChanger.instance.FadeToLevelSet(LevelChanger.USER_MENU);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game selected.");
        Application.Quit();
    }
}
