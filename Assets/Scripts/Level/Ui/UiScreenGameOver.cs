using UnityEngine;
using UnityEngine.UI;

public class UiScreenGameOver : UiScreen
{
    [SerializeField]
    Text wavesText;
    [SerializeField]
    Text scoreText;

    public override void UpdateGameOverUiWaves(int waves)
    {
        wavesText.text = "Waves cleared: " + waves.ToString();
    }

    public override void UpdateGameOverUiScore(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
