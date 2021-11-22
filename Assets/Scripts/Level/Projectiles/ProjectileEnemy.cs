using UnityEngine;
using Zenject;

public class ProjectileEnemy : Projectile
{
    public override void OnTriggerEnter(Collider other)
    {
        //var player = other.GetComponent<PlayerFacade>();

        //if (player != null && _type == BulletTypes.FromEnemy)
        //{
        //    player.TakeDamage(MoveDirection);
        //    _pool.Despawn(this);
        //}
    }

    public class Factory : PlaceholderFactory<float, float, ProjectileTypes, ProjectileEnemy>
    {
    }
}
