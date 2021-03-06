using UnityEngine;
using Zenject;

public class ProjectilePlayer : Projectile
{
    public override void OnTriggerEnter(Collider other)
    {
        // Check for enemy
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null && _type == ProjectileTypes.FromPlayer)
        {
            enemy.EnemyHit();
            _pool.Despawn(this);
        }
    }

    // Factory for player bullets
    public class Factory : PlaceholderFactory<float, float, ProjectileTypes, ProjectilePlayer>
    {
    }
}
