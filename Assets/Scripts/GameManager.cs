using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Game
{
    public struct HighScore
    {
        public string Name { get; set; }
        public int Score { get; set; }

        public HighScore(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }

    public class GameManager : MonoBehaviour
    {
        // GameManager instance
        public static GameManager Instance;

        // Session-wide variables
        public Color currentPaddleColor;
        public int currentDifficulty = 0;
        public float difficultyModifier = 0.5f;
        public int currentColorID = 0;
        public string playerName = "unknown";
        public bool haseSeenSettings = false;
        public string bestHighscoreName = "unknown";
        public int bestHighscorePoints = 0;

        private string[] difficultyNames = { "Easy", "Medium", "Hard" };
        private List<HighScore> highScores = new List<HighScore>();

        // Singleton pattern
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadHighscores();
        }

        public string GetDifficultyString()
        {
            return difficultyNames[currentDifficulty];
        }

        public List<HighScore> GetHighScores()
        {
            return highScores;
        }

        public void AddHighScore(string name, int score)
        {
            highScores.Add(new HighScore(name, score));
            List<HighScore> newscores = highScores.OrderByDescending(element => element.Score).ToList();
            highScores = newscores;
            bestHighscoreName = highScores.First().Name;
            bestHighscorePoints = highScores.First().Score;
            SaveHighscores();
            return;
        }

        [System.Serializable]
        class SaveData
        {
            public string[] highscoreNames;
            public int[] highscorePoints;
        }

        public void SaveHighscores()
        {
            if (highScores.Count > 0)
            {
                SaveData data = new SaveData();
                data.highscoreNames = new string[highScores.Count];
                data.highscorePoints = new int[highScores.Count];
                int index = 0;

                foreach (HighScore highScore in highScores)
                {
                    data.highscoreNames[index] = highScore.Name;
                    data.highscorePoints[index] = highScore.Score;
                    index++;
                    if (index == 11)
                    {
                        break;
                    }
                }

                string json = JsonUtility.ToJson(data);
                File.WriteAllText(Application.dataPath + "/highscores.json", json);
            }
        }

        public void LoadHighscores()
        {
            string path = Application.dataPath + "/highscores.json";

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                SaveData data = JsonUtility.FromJson<SaveData>(json);

                for (int i = 0; i < data.highscoreNames.Length; i++)
                {
                    highScores.Add(new HighScore(data.highscoreNames[i], data.highscorePoints[i]));
                }
            }
        }
    }
}
