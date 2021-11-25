using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class ScoreManager
{
    readonly Settings _settings;

    ScoreData scoreData;

    public ScoreManager(Settings settings)
    {
        _settings = settings;
        scoreData = new ScoreData();
    }

    public List<Score> GetHighScores()
    {
        return scoreData.scores;
    }

    public void AddScore(Score score)
    {
        // Add score to list, arrange list, trim list if more than X entries, save list.
        scoreData.scores.Add(score);
        scoreData.scores = scoreData.scores.OrderByDescending(x => x._score).ToList();

        if(scoreData.scores.Count > _settings.numberOfScores)
        {
            scoreData.scores.RemoveAt(_settings.numberOfScores);
        }

        SaveScoreList();
    }

    private void SaveScoreList()
    {
        // List to json string and save under player prefs
        String json = JsonUtility.ToJson(scoreData);
        PlayerPrefs.SetString("scoreList", json);
    }

    public void LoadScoreList()
    {
        if (!PlayerPrefs.HasKey("scoreList"))
        {
            SaveScoreList();
        }

        // Load player prefs to json string and populate list
        String json = PlayerPrefs.GetString("scoreList", "{}");
        scoreData = JsonUtility.FromJson<ScoreData>(json);
    }

    [Serializable]
    public class Settings
    {
        public int numberOfScores;
    }
}

/// <summary>
/// List of scores to save/load.
/// </summary>
[Serializable]
public class ScoreData
{
    public List<Score> scores;

    public ScoreData()
    {
        scores = new List<Score>();
    }
}
