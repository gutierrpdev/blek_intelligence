using UnityEngine;
using UnityEngine.UI;

public class UserMenu : MonoBehaviour
{

    public void StartSession()
    {
        // attempt to find an input field within the scene to extract userID from.
        InputField input = FindObjectOfType<InputField>();

        // if such an input field is found AND the length of the text provided by user is sufficient,
        // set current session's user ID to the one found and transition to instruction screen.
        if(input != null && input.text.Length > 4)
        {
            SessionManager.instance.SetUserId(input.text);
            // load main menu screen, if possible.
            LevelChanger.instance.FadeToLevelSet(LevelChanger.INSTRUCTION_SCREEN);
        }
        // in any other case, user will remain in this scene until adequate information is provided.
    }
}
