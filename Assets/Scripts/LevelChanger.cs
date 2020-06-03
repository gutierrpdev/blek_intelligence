using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public static LevelChanger instance = null;

    public Animator animator;
    private string levelToLoad;

    #region scene_types
    public const string TUTORIAL_LEVEL = "Tutorial";
    public const string SESSION_LEVEL = "Session";
    public const string MAIN_MENU = "MainMenu";
    public const string TUTORIAL_MENU = "TutorialMenu";
    public const string SESSION_MENU = "SessionMenu";
    public const string USER_MENU = "UserMenu";
    public const string INSTRUCTION_SCREEN = "InstructionScreen";
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Make sure level changer persists between scenes.
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Start()
    {
        //EventManager.TimeUp += FadeToMainMenu;
        EventManager.LevelCompleted += FadeToNextLevel;
    }

    #region fade_methods
    private void FadeToMainMenu()
    {
        FadeToLevelSet(MAIN_MENU);
    }

    public void FadeToNextLevel()
    {
        // separate level between tag (either session or tutorial) and level number.
        string[] level = SceneManager.GetActiveScene().name.Split('_');

        // if such separation is not possible, stay at current level (there isn't a 'next' level in this case).
        if (level.Length < 2) return;

        string type = level[0];
        int number = int.Parse(level[1]);

        // attemp to find scene within same type but next level index.
        levelToLoad = type + "_" + (number + 1);
        // check if levelToLoad scene exists in project.
        bool levelExists = SceneUtility.GetBuildIndexByScenePath(levelToLoad) >= 0;

        // if no such scene could be found, load main menu.
        if (!levelExists)
        {
            // if original set type was tutorial or session, we have reached the last level
            // of level set and must notify our session manager about the end of the session/tutorial.
            if(type == SESSION_LEVEL)
            {
                WebSessionManager.instance?.LevelEnd(number);
                WebSessionManager.instance?.ExperimentEnd();
            }
            else if(type == TUTORIAL_LEVEL)
            {
                levelToLoad = SESSION_MENU;
                WebSessionManager.instance?.LevelEnd(number);
                WebSessionManager.instance?.TutorialEnd();
            }
            else
            {
                levelToLoad = TUTORIAL_MENU;
            }
        }
        else if(type == SESSION_LEVEL || type == TUTORIAL_LEVEL)
        {
            // if levels are within session or tutorial sets, notify the session manager of level start/end
            // with appropriate numbers
            WebSessionManager.instance?.LevelEnd(number);
            WebSessionManager.instance?.LevelStart(number + 1);
        }

        // proceed with fadeout. end-of-animation event will trigger actual scene change with
        // level to load as determined above.
        animator.SetTrigger("FadeOut");
    }

    public void FadeToLevelSet(string type)
    {
        // attempt to load first level of selected level set.
        levelToLoad = (type == MAIN_MENU)? MAIN_MENU : 
            (type == USER_MENU ? USER_MENU :
            (type == TUTORIAL_MENU ? TUTORIAL_MENU :
            (type == SESSION_MENU ? SESSION_MENU :
            (type == INSTRUCTION_SCREEN ? INSTRUCTION_SCREEN : type + "_0"))));

        // check if levelToLoad scene exists in project.
        bool setExists = SceneUtility.GetBuildIndexByScenePath(levelToLoad) >= 0;

        // if level set is found, fade to its first level.
        // otherwise, stay at current location.
        if (setExists)
        {
            animator.SetTrigger("FadeOut");
            // Notify the session manager about level start.
            if (type == SESSION_LEVEL)
            {
                WebSessionManager.instance?.ExperimentStart();
                WebSessionManager.instance?.LevelStart(0);
            }
            else if (type == TUTORIAL_LEVEL)
            {
                WebSessionManager.instance?.TutorialStart();
                WebSessionManager.instance?.LevelStart(0);
            }
        }

    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
    #endregion

    private void OnDisable()
    {
        EventManager.TimeUp -= FadeToMainMenu;
        EventManager.LevelCompleted -= FadeToNextLevel;
    }
}
