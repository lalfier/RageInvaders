using Zenject;

public class PlayerStateDead : PlayerState
{
    readonly SignalBus _signalBus;
    readonly Player _player;

    public PlayerStateDead(
        Player player,
        SignalBus signalBus)
    {
        _signalBus = signalBus;
        _player = player;
    }

    public override void Start()
    {
        // Disable player mesh and fire a dead signal
        _player.MeshRenderer.enabled = false;
        _signalBus.Fire<PlayerDeadSignal>();
    }

    public override void Dispose()
    {
        _player.MeshRenderer.enabled = true;
    }

    public override void Update()
    {
    }

    public override void FixedUpdate()
    {
    }

    // Factory for dead state
    public class Factory : PlaceholderFactory<PlayerStateDead>
    {
    }
}
