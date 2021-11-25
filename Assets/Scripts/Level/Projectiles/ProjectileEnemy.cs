using UnityEngine;
using Zenject;

public class ProjectileEnemy : Projectile
{
    public override void OnTriggerEnter(Collider other)
    {
        // Check for player
        Player player = other.GetComponent<Player>();

        if (player != null && _type == ProjectileTypes.FromEnemy)
        {
            player.PlayerHit();
            _pool.Despawn(this);
        }
    }

    // Factory for enemy bullets
    public class Factory : PlaceholderFactory<float, float, ProjectileTypes, ProjectileEnemy>
    {
    }
}
