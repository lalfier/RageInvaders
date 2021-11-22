using UnityEngine;
using Zenject;

public class ProjectilePlayer : Projectile
{
    public override void OnTriggerEnter(Collider other)
    {
        //var enemyView = other.GetComponent<EnemyView>();

        //if (enemyView != null && _type == BulletTypes.FromPlayer)
        //{
        //    enemyView.Facade.Die();
        //    _pool.Despawn(this);
        //}
    }

    public class Factory : PlaceholderFactory<float, float, ProjectileTypes, ProjectilePlayer>
    {
    }
}
