using System;
using ModestTree;
using UnityEngine;
using Zenject;

public class PlayerStatePlaying : PlayerState
{
    readonly LevelBounds _levelBoundary;
    readonly Settings _settings;
    readonly Player _player;

    public PlayerStatePlaying(
        Player player,
        Settings settings,
        LevelBounds levelBoundary)
    {
        _levelBoundary = levelBoundary;
        _settings = settings;
        _player = player;
    }

    public override void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        var isMovigLeft = Input.GetAxisRaw("Horizontal") < 0;
        var isMovigRight = Input.GetAxisRaw("Horizontal") > 0;

        if (isMovigLeft)
        {
            _player.Rigidbody.AddForce(Vector3.left * -_settings.MoveSpeed);
        }

        if (isMovigRight)
        {
            _player.Rigidbody.AddForce(Vector3.right * -_settings.MoveSpeed);
        }

        // Always ensure we are on the main plane
        _player.Position = new Vector3(_player.Position.x, 0, _player.Position.z);

        KeepPlayerOnScreen();
    }

    void KeepPlayerOnScreen()
    {
        var extentLeft = (_levelBoundary.Left + _settings.BoundaryBuffer) - _player.Position.x;
        var extentRight = _player.Position.x - (_levelBoundary.Right - _settings.BoundaryBuffer);

        if (extentLeft > 0)
        {
            _player.Rigidbody.AddForce(
                Vector3.right * _settings.BoundaryAdjustForce * extentLeft);
        }
        else if (extentRight > 0)
        {
            _player.Rigidbody.AddForce(
                Vector3.left * _settings.BoundaryAdjustForce * extentRight);
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        Assert.That(other.GetComponent<Projectile>() != null);
        _player.ChangeState(PlayerStates.Dead);
    }

    public override void Update()
    {
    }

    public override void Start()
    {
    }

    public override void Dispose()
    {
    }

    [Serializable]
    public class Settings
    {
        public float BoundaryBuffer;
        public float BoundaryAdjustForce;
        public float MoveSpeed;
    }

    public class Factory : PlaceholderFactory<PlayerStatePlaying>
    {
    }
}
