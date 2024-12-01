using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject[] panels = new GameObject[2];
    public GameObject[] paddleButtons = new GameObject[5];

    private int currentPanel, currentPaddleColor;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;

        currentPaddleColor = gameManager.GetPaddleColorIndex();
        SetPaddleColor(currentPaddleColor);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenPanel(int panelID)
    {
        panels[panelID].SetActive(true);
        currentPanel = panelID;
    }

    public void ClosePanel()
    {
        panels[currentPanel].SetActive(false);
    }

    public void SetPaddleColor(int colorID)
    {
        paddleButtons[currentPaddleColor].GetComponent<Outline>().enabled = false;
        paddleButtons[colorID].GetComponent<Outline>().enabled = true;
        currentPaddleColor = colorID;
        gameManager.SetPaddleInfo(paddleButtons[currentPaddleColor].GetComponent<Image>().color, currentPaddleColor);
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
