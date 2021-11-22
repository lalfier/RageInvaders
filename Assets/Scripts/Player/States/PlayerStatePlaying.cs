using System;
using UnityEngine;
using Zenject;

public class PlayerStatePlaying : PlayerState
{
    readonly LevelBounds _levelBoundary;
    readonly Settings _settings;
    readonly Player _player;
    readonly ProjectilePlayer.Factory _bulletFactory;
    readonly SignalBus _signalBus;

    float _lastFireTime;
    int _currentLives;

    public PlayerStatePlaying(
        Player player, ProjectilePlayer.Factory bulletFactory,
        Settings settings, SignalBus signalBus,
        LevelBounds levelBoundary)
    {
        _levelBoundary = levelBoundary;
        _settings = settings;
        _player = player;
        _bulletFactory = bulletFactory;
        _signalBus = signalBus;
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

    void Fire()
    {
        var bullet = _bulletFactory.Create(
            _settings.BulletSpeed, _settings.BulletLifetime, ProjectileTypes.FromPlayer);

        bullet.transform.position = _player.Position + _player.LookDir * _settings.BulletOffsetDistance;
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

    public override void PlayerHit()
    {
        _currentLives--;
        _signalBus.Fire(new PlayerLivesSignal() { currentLives = _currentLives });

        if (_currentLives == 0)
        {
            _player.ChangeState(PlayerStates.Dead);
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if(enemy != null)
        {
            PlayerHit();
        }        
    }

    public override void Update()
    {
        bool isFiring = Input.GetButton("Fire1");
        if (isFiring && Time.realtimeSinceStartup - _lastFireTime > _settings.MaxShootInterval)
        {
            _lastFireTime = Time.realtimeSinceStartup;
            Fire();
        }

        // Cheat
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerHit();
        }
    }

    public override void Start()
    {
        _currentLives = _settings.Lives;
        _signalBus.Fire(new PlayerLivesSignal() { currentLives = _currentLives });
    }

    public override void Dispose()
    {
    }

    [Serializable]
    public class Settings
    {
        public int Lives;
        public float MoveSpeed;
        public float BoundaryBuffer;
        public float BoundaryAdjustForce;
        public float BulletLifetime;
        public float BulletSpeed;
        public float MaxShootInterval;
        public float BulletOffsetDistance;
    }

    public class Factory : PlaceholderFactory<PlayerStatePlaying>
    {
    }
}
