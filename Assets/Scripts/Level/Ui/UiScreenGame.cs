using UnityEngine;
using UnityEngine.UI;

public class UiScreenGame : UiScreen
{
    [SerializeField]
    Text livesText;
    [SerializeField]
    Text wavesText;
    [SerializeField]
    Text scoreText;

    public override void UpdatePlayingUiLives(int lives)
    {
        livesText.text = "L: " + lives.ToString();
    }

    public override void UpdatePlayingUiWaves(int waves)
    {
        wavesText.text = "W: " + waves.ToString();
    }

    public override void UpdatePlayingUiScore(int score)
    {
        scoreText.text = "S: " + score.ToString();
    }
}
