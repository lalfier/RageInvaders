using UnityEngine;
using Zenject;

public enum ProjectileTypes
{
    FromEnemy,
    FromPlayer
}

public class Projectile : MonoBehaviour, IPoolable<float, float, ProjectileTypes, IMemoryPool>
{
    float _startTime;
    ProjectileTypes _type;
    float _speed;
    float _lifeTime;

    [SerializeField]
    MeshRenderer _renderer = null;

    [SerializeField]
    Material _playerMaterial = null;

    [SerializeField]
    Material _enemyMaterial = null;

    IMemoryPool _pool;

    public ProjectileTypes Type
    {
        get { return _type; }
    }

    public Vector3 MoveDirection
    {
        get { return transform.right; }
    }

    public void OnTriggerEnter(Collider other)
    {
        //var enemyView = other.GetComponent<EnemyView>();

        //if (enemyView != null && _type == BulletTypes.FromPlayer)
        //{
        //    enemyView.Facade.Die();
        //    _pool.Despawn(this);
        //}
        //else
        //{
        //    var player = other.GetComponent<PlayerFacade>();

        //    if (player != null && _type == BulletTypes.FromEnemy)
        //    {
        //        player.TakeDamage(MoveDirection);
        //        _pool.Despawn(this);
        //    }
        //}
    }

    public void Update()
    {
        transform.position -= transform.right * _speed * Time.deltaTime;

        if (Time.realtimeSinceStartup - _startTime > _lifeTime)
        {
            _pool.Despawn(this);
        }
    }

    public void OnSpawned(float speed, float lifeTime, ProjectileTypes type, IMemoryPool pool)
    {
        _pool = pool;
        _type = type;
        _speed = speed;
        _lifeTime = lifeTime;

        _renderer.material = type == ProjectileTypes.FromEnemy ? _enemyMaterial : _playerMaterial;

        _startTime = Time.realtimeSinceStartup;
    }

    public void OnDespawned()
    {
        _pool = null;
    }

    public class Factory : PlaceholderFactory<float, float, ProjectileTypes, Projectile>
    {
    }
}
