using System;
using UnityEngine;
using Zenject;

public class PlayerStateWaiting : PlayerState
{
    readonly Settings _settings;
    readonly Player _player;
    readonly LevelBounds _levelBoundary;

    float _theta;
    Vector3 startingPos;

    public PlayerStateWaiting(
        Player player,
        Settings settings, LevelBounds levelBoundary)
    {
        _settings = settings;
        _player = player;
        _levelBoundary = levelBoundary;
    }

    public override void Start()
    {
        float zPos = _levelBoundary.Bottom + 1;
        _player.Position = new Vector3(_player.Position.x, 0, -zPos);
        _player.Position += _settings.StartOffset;
        startingPos = _player.Position;
    }

    public override void Update()
    {
        _player.Position = startingPos + Vector3.right * _settings.Amplitude * Mathf.Sin(_theta);
        _theta += Time.deltaTime * _settings.Frequency;
    }

    public override void FixedUpdate()
    {
    }

    [Serializable]
    public class Settings
    {
        public Vector3 StartOffset;
        public float Amplitude;
        public float Frequency;
    }

    public class Factory : PlaceholderFactory<PlayerStateWaiting>
    {
    }
}
