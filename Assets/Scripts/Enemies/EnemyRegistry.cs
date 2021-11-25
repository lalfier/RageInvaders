using System.Collections.Generic;

/// <summary>
/// Keep track of live enemies on screen.
/// </summary>
public class EnemyRegistry
{
    readonly List<Enemy> _enemies = new List<Enemy>();

    public List<Enemy> Enemies
    {
        get { return _enemies; }
    }

    public int EnemyCount
    {
        get { return _enemies.Count; }
    }

    public void AddEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
    }
}
