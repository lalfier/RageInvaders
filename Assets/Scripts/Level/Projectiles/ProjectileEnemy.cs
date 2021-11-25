using UnityEngine;
using Zenject;

public class ProjectileEnemy : Projectile
{
    public override void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null && _type == ProjectileTypes.FromEnemy)
        {
            player.PlayerHit();
            _pool.Despawn(this);
        }
    }

    public class Factory : PlaceholderFactory<float, float, ProjectileTypes, ProjectileEnemy>
    {
    }
}
