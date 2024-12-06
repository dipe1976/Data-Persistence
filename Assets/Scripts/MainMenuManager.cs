using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Game;

public class MainMenuManager : MonoBehaviour
{
    public GameObject[] panels = new GameObject[2];
    public GameObject[] paddleButtons = new GameObject[5];
    public TMP_Text highScoreText;
    public TMP_Text settingsString;
    public TMP_Dropdown difficultyDropdown;
    public TMP_InputField nameField;

    private GameManager gameManager;
    private int currentPanel;

    void Start()
    {
        gameManager = GameManager.Instance;
        SetPaddleColor(gameManager.currentColorID);
        UpdateSettingsString();
        UpdateHighscoreList();
        if (!gameManager.haseSeenSettings)
        {
            OpenPanel(1);
            gameManager.haseSeenSettings = true;
        }
        else
        {
            nameField.text = gameManager.playerName;
            difficultyDropdown.value = gameManager.currentDifficulty;
        }

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

    public void SetPlayerName()
    {
        gameManager.playerName = nameField.text;
        UpdateSettingsString();
    }

    public void SetDifficulty()
    {
        gameManager.currentDifficulty = difficultyDropdown.value;
        gameManager.difficultyModifier = ((float)difficultyDropdown.value+1.0f) / 2.0f;
        UpdateSettingsString();
    }

    public void SetPaddleColor(int colorID)
    {
        paddleButtons[gameManager.currentColorID].GetComponent<Outline>().enabled = false;
        paddleButtons[colorID].GetComponent<Outline>().enabled = true;
        gameManager.currentColorID = colorID;
        gameManager.currentPaddleColor = paddleButtons[colorID].GetComponent<Image>().color;
    }

    public void UpdateSettingsString()
    {
        settingsString.text = $"Your name: {gameManager.playerName}\nDifficulty: {gameManager.GetDifficultyString()}";
    }

    public void UpdateHighscoreList()
    {
        int currentPlace = 1;
        highScoreText.text = string.Empty;

        foreach (HighScore highscore in gameManager.GetHighScores())
        {
            highScoreText.text += $"#{currentPlace} - {highscore.Name} | {highscore.Score} points";
            if (currentPlace != 10)
            {
                highScoreText.text += "\n";
            }
            currentPlace++;
            if (currentPlace == 11)
            {
                break;
            }
        }
        if (currentPlace <= 10)
        {
            for (int i = currentPlace; i <= 10; i++)
            {
                highScoreText.text += $"#{i} - no one";
                if (i != 10)
                {
                    highScoreText.text += "\n";
                }
            }
        }
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
