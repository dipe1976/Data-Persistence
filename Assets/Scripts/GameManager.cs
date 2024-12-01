using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Color currentPaddleColor;
    public int currentDifficulty, currentColorID;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        currentColorID = 0;
    }
    public void SetPaddleInfo(Color color, int colorID)
    {
        currentPaddleColor = color;
        currentColorID = colorID;
    }

    public Color GetPaddleColor()
    {
        return currentPaddleColor;
    }

    public int GetPaddleColorIndex()
    {
        return currentColorID;
    }

    public void SetDifficulty(int difficulty)
    {
        currentDifficulty = difficulty;
    }

    public int GetDifficulty()
    {
        return currentDifficulty;
    }

}
