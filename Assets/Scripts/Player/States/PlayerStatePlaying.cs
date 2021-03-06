using System;
using UnityEngine;
using Zenject;

public class PlayerStatePlaying : PlayerState
{
    readonly LevelBounds _levelBoundary;
    readonly Settings _settings;
    readonly Player _player;
    readonly ProjectilePlayer.Factory _projectileFactory;
    readonly SignalBus _signalBus;

    float _lastFireTime;
    int _currentLives;
    float _currentInvulnerableTime;

    public PlayerStatePlaying(
        Player player, ProjectilePlayer.Factory projectileFactory,
        Settings settings, SignalBus signalBus,
        LevelBounds levelBoundary)
    {
        _levelBoundary = levelBoundary;
        _settings = settings;
        _player = player;
        _projectileFactory = projectileFactory;
        _signalBus = signalBus;
    }

    public override void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // Move player with input
        bool isMovigLeft = SimpleInput.GetAxisRaw("Horizontal") < 0;
        bool isMovigRight = SimpleInput.GetAxisRaw("Horizontal") > 0;

        if (isMovigLeft)
        {
            _player.Rigidbody.AddForce(Vector3.left * -_settings.moveSpeed);
        }

        if (isMovigRight)
        {
            _player.Rigidbody.AddForce(Vector3.right * -_settings.moveSpeed);
        }

        // Always ensure we are on the main plane
        _player.Position = new Vector3(_player.Position.x, 0, _player.Position.z);

        KeepPlayerOnScreen();
    }

    void Fire()
    {
        // Create bullets from factory with speed and lifetime
        ProjectilePlayer projectile = _projectileFactory.Create(
            _settings.bulletSpeed, _settings.bulletLifetime, ProjectileTypes.FromPlayer);

        projectile.transform.position = _player.Position + _player.LookDir * _settings.bulletOffsetDistance;
    }

    void KeepPlayerOnScreen()
    {
        // Keep player inside level bounds
        float extentLeft = (_levelBoundary.Left + _settings.boundaryBuffer) - _player.Position.x;
        float extentRight = _player.Position.x - (_levelBoundary.Right - _settings.boundaryBuffer);

        if (extentLeft > 0)
        {
            _player.Rigidbody.AddForce(
                Vector3.right * _settings.boundaryAdjustForce * extentLeft);
        }
        else if (extentRight > 0)
        {
            _player.Rigidbody.AddForce(
                Vector3.left * _settings.boundaryAdjustForce * extentRight);
        }
    }

    public override void PlayerHit()
    {
        // Player can not be damaged while invulnerable
        if (_currentInvulnerableTime > 0)
        {
            return;
        }

        // Fire hit signal with remaining lives
        _currentLives--;
        _currentInvulnerableTime = _settings.invulnerableTime;
        _signalBus.Fire(new PlayerLivesSignal() { currentLives = _currentLives });

        if (_currentLives == 0)
        {
            // If dead change state
            _player.ChangeState(PlayerStates.Dead);
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if(enemy != null)
        {
            PlayerHit();
        }        
    }

    public override void Update()
    {
        // Fire with input every x seconds
        bool isFiring = SimpleInput.GetButton("Fire1");
        if (isFiring && Time.realtimeSinceStartup - _lastFireTime > _settings.maxShootInterval)
        {
            _lastFireTime = Time.realtimeSinceStartup;
            Fire();
        }

        if (_currentInvulnerableTime > 0)
        {
            _currentInvulnerableTime -= Time.deltaTime;
        }
    }

    public override void Start()
    {
        _currentLives = _settings.lives;
        _currentInvulnerableTime = 0;
        _signalBus.Fire(new PlayerLivesSignal() { currentLives = _currentLives });
    }

    [Serializable]
    public class Settings
    {
        public int lives;
        public float invulnerableTime;
        public float moveSpeed;
        public float boundaryBuffer;
        public float boundaryAdjustForce;
        public float bulletLifetime;
        public float bulletSpeed;
        public float maxShootInterval;
        public float bulletOffsetDistance;
    }

    // Factory for play state
    public class Factory : PlaceholderFactory<PlayerStatePlaying>
    {
    }
}
