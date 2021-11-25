using System;
using UnityEngine;
using Zenject;

public enum EnemyTypes
{
    Type1,
    Type2,
    Type3
}

public class Enemy : MonoBehaviour, IPoolable<IMemoryPool>
{
    [SerializeField]
    EnemyTypes _type;

    Settings _settings;
    ProjectileEnemy.Factory _projectileFactory;
    SignalBus _signalBus;
    IMemoryPool _pool;
    EnemyRegistry _registry;

    int _currentLives;

    [Inject]
    public void Construct(
        ProjectileEnemy.Factory projectileFactory,
        Settings settings, SignalBus signalBus,
        IMemoryPool pool, EnemyRegistry registry)
    {
        _settings = settings;
        _projectileFactory = projectileFactory;
        _signalBus = signalBus;
        _pool = pool;
        _registry = registry;
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
        // Create bullets from factory with speed and lifetime
        ProjectileEnemy projectile = _projectileFactory.Create(
            _settings.bulletSpeed, _settings.bulletLifetime, ProjectileTypes.FromEnemy);

        projectile.transform.position = Position + LookDir * _settings.bulletOffsetDistance;
    }

    public void EnemyHit()
    {
        _currentLives--;        

        if (_currentLives == 0)
        {
            // Send enemy dead signal with score and remove enemy from screen
            int score = (int)_type * _settings.scoreOnKill + _settings.scoreOnKill;
            _signalBus.Fire(new EnemyDeadSignal() { typeScore = score });
            Despawn();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            EnemyHit();
        }
    }

    public void Despawn()
    {
        _pool.Despawn(this);
    }

    public void OnDespawned()
    {
        // Remove enemy from registry on killed
        _registry.RemoveEnemy(this);
        _pool = null;
    }

    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
        _currentLives = _settings.lives;
        // Add enemy to registry on created
        _registry.AddEnemy(this);
    }

    [Serializable]
    public class Settings
    {
        public int lives;
        public float bulletLifetime;
        public float bulletSpeed;
        public float bulletOffsetDistance;
        public int scoreOnKill;
    }

    // Factory
    public class Factory : PlaceholderFactory<Enemy>
    {
    }
}
