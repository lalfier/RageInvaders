using System;
using UnityEngine;
using Zenject;

public class PlayerStateWaiting : PlayerState
{
    readonly Settings _settings;
    readonly Player _player;
    readonly LevelBounds _levelBoundary;

    float _theta;
    Vector3 _startingPos;

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
        _player.Position = new Vector3(0, 0, -zPos);
        _player.Position += _settings.startOffset;
        _startingPos = _player.Position;
    }

    public override void Update()
    {
        _player.Position = _startingPos + Vector3.right * _settings.amplitude * Mathf.Sin(_theta);
        _theta += Time.deltaTime * _settings.frequency;
    }

    public override void FixedUpdate()
    {
    }

    [Serializable]
    public class Settings
    {
        public Vector3 startOffset;
        public float amplitude;
        public float frequency;
    }

    public class Factory : PlaceholderFactory<PlayerStateWaiting>
    {
    }
}
