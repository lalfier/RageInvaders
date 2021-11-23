using System;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour, IPoolable<IMemoryPool>
{
    Settings _settings;
    ProjectileEnemy.Factory _projectileFactory;
    SignalBus _signalBus;
    IMemoryPool _pool;

    int _currentLives;

    [Inject]
    public void Construct(
        ProjectileEnemy.Factory projectileFactory,
        Settings settings, SignalBus signalBus,
        IMemoryPool pool)
    {
        _settings = settings;
        _projectileFactory = projectileFactory;
        _signalBus = signalBus;
        _pool = pool;
    }

    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public Quaternion Rotation
    {
        get { return transform.rotation; }
        set { transform.rotation = value; }
    }

    public Vector3 LookDir
    {
        get { return transform.forward; }
    }

    public void Fire()
    {
        var projectile = _projectileFactory.Create(
            _settings.BulletSpeed, _settings.BulletLifetime, ProjectileTypes.FromEnemy);

        projectile.transform.position = Position + LookDir * _settings.BulletOffsetDistance;
    }

    public void EnemyHit()
    {
        _currentLives--;        

        if (_currentLives == 0)
        {
            _signalBus.Fire<EnemyDeadSignal>();
            _pool.Despawn(this);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            EnemyHit();
        }
    }

    public void OnDespawned()
    {        
        _pool = null;
    }

    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
        _currentLives = _settings.Lives;
    }

    [Serializable]
    public class Settings
    {
        public int Lives;
        public float BulletLifetime;
        public float BulletSpeed;
        public float BulletOffsetDistance;
    }

    public class Factory : PlaceholderFactory<Enemy>
    {
    }
}
