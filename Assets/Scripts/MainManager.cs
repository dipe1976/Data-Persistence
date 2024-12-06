using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Game;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 1;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    public GameObject ExitPanel;
    public GameObject Paddle;
    
    private bool m_Started = false;
    private bool m_Paused = false;
    private bool m_GameOver = false;
    private int m_Points = 0;
    public int brickCount = 0;

    private GameManager gameManager;

    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        Paddle.GetComponent<Renderer>().material.SetColor("_BaseColor", gameManager.currentPaddleColor);
        HighScoreText.text = $"Best score: {gameManager.bestHighscoreName} | {gameManager.bestHighscorePoints} points";

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
                brickCount++;
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * (2.0f * gameManager.difficultyModifier), ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !m_Paused)
        {
            m_Paused = true;
            Time.timeScale = 0.0f;
            ExitPanel.SetActive(true);
        }
    }

    void AddPoint(int point)
    {
        m_Points += Mathf.RoundToInt((float)point * gameManager.difficultyModifier);
        ScoreText.text = $"Score : {m_Points}";
        brickCount--;
        if (brickCount == 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        Ball.constraints = RigidbodyConstraints.FreezeAll;
        m_GameOver = true;
        GameOverText.SetActive(true);
        gameManager.AddHighScore(gameManager.playerName, m_Points);
        HighScoreText.text = $"Best score: {gameManager.bestHighscoreName} | {gameManager.bestHighscorePoints} points";
    }

    public void BackToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void BackToGame()
    {
        m_Paused = false;
        Time.timeScale = 1.0f;
        ExitPanel.SetActive(false);
    }
}
