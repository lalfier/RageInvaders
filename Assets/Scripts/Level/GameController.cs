using ModestTree;
using System;
using UnityEngine;
using Zenject;

public enum GameStates
{
    MainMenu,
    Playing,
    GameOverMenu
}

public class GameController : IInitializable, ITickable, IDisposable
{
    readonly SignalBus _signalBus;
    readonly EnemyManager _enemyManager;
    readonly Player _player;
    readonly UiManager _uiManager;
    readonly ScoreManager _scoreManager;

    GameStates _state = GameStates.MainMenu;
    UiScreen _currentUiScrren;
    int _currentWave;
    int _currentScore;

    public GameController(
        Player player, EnemyManager enemyManager,
        SignalBus signalBus, UiManager uiManager,
        ScoreManager scoreManager)
    {
        _signalBus = signalBus;
        _enemyManager = enemyManager;
        _player = player;
        _uiManager = uiManager;
        _scoreManager = scoreManager;
    }

    public GameStates State
    {
        get { return _state; }
    }

    public void Initialize()
    {
        Physics.gravity = Vector3.zero;
        _scoreManager.LoadScoreList();
        _signalBus.Subscribe<PlayerDeadSignal>(OnPlayerDied);
        _signalBus.Subscribe<PlayerLivesSignal>(OnPlayerHit);
        _signalBus.Subscribe<EnemyDeadSignal>(OnEnemyKilled);
        _signalBus.Subscribe<WaveCreatedSignal>(OnWaveCreated);
        _signalBus.Subscribe<StartButtonSignal>(StartGame);
        _signalBus.Subscribe<MenuButtonSignal>(SetMainMenu);
        _signalBus.Subscribe<ScoresButtonSignal>(ShowHighScores);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<PlayerDeadSignal>(OnPlayerDied);
        _signalBus.Unsubscribe<PlayerLivesSignal>(OnPlayerHit);
        _signalBus.Unsubscribe<EnemyDeadSignal>(OnEnemyKilled);
        _signalBus.Unsubscribe<WaveCreatedSignal>(OnWaveCreated);
        _signalBus.Unsubscribe<StartButtonSignal>(StartGame);
        _signalBus.Unsubscribe<MenuButtonSignal>(SetMainMenu);
        _signalBus.Unsubscribe<ScoresButtonSignal>(ShowHighScores);
    }

    public void Tick()
    {
        switch (_state)
        {
            case GameStates.MainMenu:
                {
                    UpdateStarting();
                    break;
                }
            case GameStates.Playing:
                {
                    UpdatePlaying();
                    break;
                }
            case GameStates.GameOverMenu:
                {
                    UpdateGameOver();
                    break;
                }
            default:
                {
                    Assert.That(false);
                    break;
                }
        }
    }

    void SetMainMenu()
    {
        if(_state != GameStates.MainMenu)
        {
            // If we came from other state
            _enemyManager.Exit();
            _player.ChangeState(PlayerStates.Waiting);
            _state = GameStates.MainMenu;
        }
        // Set main menu as active
        _currentUiScrren = _uiManager.ActivateUiPanel(UiTypes.MainMenuUi);
    }

    void UpdateStarting()
    {
        Assert.That(_state == GameStates.MainMenu);
        // Main Menu
    }

    void OnPlayerDied()
    {
        // Set game over menu and populate text fields
        Assert.That(_state == GameStates.Playing);
        _state = GameStates.GameOverMenu;
        _enemyManager.Stop();
        _currentUiScrren = _uiManager.ActivateUiPanel(UiTypes.GameOverUi);
        _currentUiScrren.UpdateGameOverUiWaves(_currentWave - 1);
        _currentUiScrren.UpdateGameOverUiScore(_currentScore);

        // Add score to score manager
        string date = DateTime.Now.ToString("dd/MM/yyyy");
        _scoreManager.AddScore(new Score(date, _currentScore));
    }

    void UpdateGameOver()
    {
        Assert.That(_state == GameStates.GameOverMenu);
        // Game Over Menu
    }

    void StartGame()
    {
        // Reset game, set playing ui and start enemy waves
        Assert.That(_state == GameStates.MainMenu);
        _currentWave = 0;
        _currentScore = 0;
        _state = GameStates.Playing;
        _currentUiScrren = _uiManager.ActivateUiPanel(UiTypes.PlayingUi);
        _player.ChangeState(PlayerStates.Playing);
        _enemyManager.Start();
        _currentUiScrren.UpdatePlayingUiScore(_currentScore);
    }

    void UpdatePlaying()
    {
        Assert.That(_state == GameStates.Playing);
        // Playing
    }

    void ShowHighScores()
    {
        // Populate high score screen with best scores
        Assert.That(_state == GameStates.MainMenu);
        _currentUiScrren = _uiManager.ActivateUiPanel(UiTypes.ScoresUi);
        _currentUiScrren.UpdateHighScoresUi(_uiManager.GetRowPrefab(), _scoreManager.GetHighScores());
    }

    void OnPlayerHit(PlayerLivesSignal livesInfo)
    {
        // Update lives on playing ui
        Assert.That(_state == GameStates.Playing);
        _currentUiScrren.UpdatePlayingUiLives(livesInfo.currentLives);
    }

    void OnEnemyKilled(EnemyDeadSignal scoreInfo)
    {
        // Update score on playing ui
        if (_state == GameStates.Playing)
        {
            _currentScore += scoreInfo.typeScore;
            _currentUiScrren.UpdatePlayingUiScore(_currentScore);
        }
    }

    void OnWaveCreated()
    {
        // Update wave on playing ui
        Assert.That(_state == GameStates.Playing);
        _currentWave++;
        _currentUiScrren.UpdatePlayingUiWaves(_currentWave);
    }
}
