using System;

/// <summary>
/// Data for single score entry.
/// </summary>
[Serializable]
public class Score
{
    public string _date;
    public int _score;

    public Score(string date, int score)
    {
        _date = date;
        _score = score;
    }
}
