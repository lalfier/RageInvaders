using ModestTree;

public enum PlayerStates
{
    Playing,
    Dead,
    Waiting,
    Count
}

public class PlayerStateFactory
{
    readonly PlayerStateWaiting.Factory _waitingFactory;
    readonly PlayerStatePlaying.Factory _playingFactory;
    readonly PlayerStateDead.Factory _deadFactory;

    public PlayerStateFactory(
        PlayerStateDead.Factory deadFactory,
        PlayerStatePlaying.Factory playingFactory,
        PlayerStateWaiting.Factory waitingFactory)
    {
        _waitingFactory = waitingFactory;
        _playingFactory = playingFactory;
        _deadFactory = deadFactory;
    }

    public PlayerState CreateState(PlayerStates state)
    {
        switch (state)
        {
            case PlayerStates.Dead:
                {
                    return _deadFactory.Create();
                }
            case PlayerStates.Waiting:
                {
                    return _waitingFactory.Create();
                }
            case PlayerStates.Playing:
                {
                    return _playingFactory.Create();
                }
        }

        throw Assert.CreateException();
    }
}
