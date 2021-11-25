using System;

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
