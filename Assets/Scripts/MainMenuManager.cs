using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject highscorePanel;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OpenHighscores()
    {
        highscorePanel.SetActive(true);
    }

    public void CloseHighscore()
    {
        highscorePanel.SetActive(false);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
