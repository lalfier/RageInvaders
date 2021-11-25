using UnityEngine;
using System.Collections.Generic;

public class UiScreenScores : UiScreen
{
    [SerializeField]
    Transform table;

    public override void UpdateHighScoresUi(GameObject rowPref, List<Score> scores)
    {
        // Clear all high score rows from table
        foreach (Transform child in table)
        {
            Destroy(child.gameObject);
        }

        // Populate high score table with rows from loaded list
        foreach (Score score in scores)
        {
            UiRow row = Instantiate(rowPref, table).GetComponent<UiRow>();
            row.date.text = score._date;
            row.score.text = score._score.ToString();
        }
    }
}
