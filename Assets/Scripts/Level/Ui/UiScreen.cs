using UnityEngine;

public class UiScreen : MonoBehaviour
{
    public virtual void UpdatePlayingUiLives(int lives)
    {
        // optionally overridden
    }

    public virtual void UpdatePlayingUiWaves(int waves)
    {
        // optionally overridden
    }

    public virtual void UpdatePlayingUiScore(int score)
    {
        // optionally overridden
    }
}
