using Zenject;

public class EnemyManager : ITickable, IFixedTickable
{
    readonly Enemy.Factory _enemyFactory;
    readonly SignalBus _signalBus;

    public EnemyManager(
        Enemy.Factory enemyFactory, SignalBus signalBus)
    {
        _enemyFactory = enemyFactory;
        _signalBus = signalBus;
    }

    public void FixedTick()
    {

    }

    public void Tick()
    {

    }

    public void Stop()
    {
        _signalBus.Unsubscribe<EnemyDeadSignal>(OnEnemyKilled);
    }

    public void Start()
    {
        _signalBus.Subscribe<EnemyDeadSignal>(OnEnemyKilled);
    }

    void OnEnemyKilled()
    {
        
    }
}
