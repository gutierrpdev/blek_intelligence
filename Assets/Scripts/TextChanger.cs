using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChanger : MonoBehaviour
{
    public List<string> texts;
    public Text textField;
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        if(texts.Count > 0)
        {
            textField.text = texts[0];
        }
    }

    public void NextText()
    {
        Debug.Log("Next text");
        index++;
        if(index < texts.Count)
        {
            textField.text = texts[index];
        }
        else
        {
            LevelChanger.instance.FadeToLevelSet(LevelChanger.MAIN_MENU);
        }
    }
}
