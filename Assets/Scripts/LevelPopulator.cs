using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelPopulator : MonoBehaviour
{

    private int sceneCount;
    public GameObject levelButtonPrefab;
    public GameObject contentPanel;
    public LevelSelector selector;

    void Start()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings;
        if (!levelButtonPrefab)
        {
            Debug.Log("No Level Button Prefab was assigned to LevelPopulator.");
            Application.Quit();
        }
        else if (!selector)
        {
            selector = FindObjectOfType<LevelSelector>();
            if (!selector)
            {
                Debug.Log("No Level Selector was assigned to LevelSelector or found in scene.");
                Application.Quit();
            }
        }
        PopulateButtons();
    }

    // Populate content panel with buttons assigned to scenes. 
    private void PopulateButtons()
    {
        Debug.Log("scene count " + sceneCount);
        for(int i = 1; i < sceneCount; i++)
        {
            // instantiate a button per scene available in build (except menu scene).
            GameObject obj = Instantiate(levelButtonPrefab);
            // place button under content panel.
            obj.transform.SetParent(contentPanel.transform, false);
            Button button = obj.GetComponent<Button>();

            int val = i;
            // select associated scene when button gets clicked.
            button.onClick.AddListener(delegate { selector.Select(val);});
            // change button text to reflect level number.
            button.GetComponentInChildren<Text>().text = i.ToString();
        }
    }
}
