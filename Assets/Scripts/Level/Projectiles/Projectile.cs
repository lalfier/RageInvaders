using UnityEngine;
using Zenject;

public enum ProjectileTypes
{
    FromEnemy,
    FromPlayer
}

public abstract class Projectile : MonoBehaviour, IPoolable<float, float, ProjectileTypes, IMemoryPool>
{
    protected float _startTime;
    protected ProjectileTypes _type;
    protected float _speed;
    protected float _lifeTime;
    protected Vector3 _direction;
    protected IMemoryPool _pool;

    public virtual ProjectileTypes Type
    {
        get { return _type; }
    }

    public virtual Vector3 MoveDirection
    {
        get { return _direction; }
    }

    public abstract void OnTriggerEnter(Collider other);

    public virtual void Update()
    {
        transform.position -= _direction * _speed * Time.deltaTime;

        // Remove after some time
        if (Time.realtimeSinceStartup - _startTime > _lifeTime)
        {
            _pool.Despawn(this);
        }
    }

    public virtual void OnSpawned(float speed, float lifeTime, ProjectileTypes type, IMemoryPool pool)
    {
        _pool = pool;
        _type = type;
        _speed = speed;
        _direction = type == ProjectileTypes.FromEnemy ? -transform.forward : transform.forward;
        _lifeTime = lifeTime;
        _startTime = Time.realtimeSinceStartup;
    }

    public virtual void OnDespawned()
    {
        _pool = null;
    }
}
