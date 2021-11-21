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

    GameStates _state = GameStates.MainMenu;
    float _elapsedTime;

    public GameController(
        Player player, EnemyManager enemyManager,
        SignalBus signalBus)
    {
        _signalBus = signalBus;
        _enemyManager = enemyManager;
        _player = player;
    }

    public float ElapsedTime
    {
        get { return _elapsedTime; }
    }

    public GameStates State
    {
        get { return _state; }
    }

    public void Initialize()
    {
        Physics.gravity = Vector3.zero;
        _signalBus.Subscribe<PlayerDeadSignal>(OnPlayerDied);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<PlayerDeadSignal>(OnPlayerDied);
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

    void UpdateGameOver()
    {
        Assert.That(_state == GameStates.GameOverMenu);
        // Game Over Menu
    }

    void OnPlayerDied()
    {
        Assert.That(_state == GameStates.Playing);
        _state = GameStates.GameOverMenu;
        _enemyManager.Stop();
    }

    void UpdatePlaying()
    {
        Assert.That(_state == GameStates.Playing);
        _elapsedTime += Time.deltaTime;
    }

    void UpdateStarting()
    {
        Assert.That(_state == GameStates.MainMenu);
        // Main Menu
        if (Input.GetButtonDown("Fire1"))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        Assert.That(_state == GameStates.MainMenu || _state == GameStates.GameOverMenu);
        
        _elapsedTime = 0;
        _enemyManager.Start();
        _player.ChangeState(PlayerStates.Playing);
        _state = GameStates.Playing;
    }
}
