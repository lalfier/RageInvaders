using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [SerializeField]
    MeshRenderer _meshRenderer;
    [SerializeField]
    Rigidbody _rigidBody;

    PlayerStateFactory _stateFactory;
    PlayerState _state;

    [Inject]
    public void Construct(PlayerStateFactory stateFactory)
    {
        _stateFactory = stateFactory;
    }

    public MeshRenderer MeshRenderer
    {
        get { return _meshRenderer; }
    }

    public Rigidbody Rigidbody
    {
        get { return _rigidBody; }
    }

    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public Quaternion Rotation
    {
        get { return transform.rotation; }
        set { transform.rotation = value; }
    }

    public void Start()
    {
        ChangeState(PlayerStates.Waiting);
    }

    public void Update()
    {
        _state.Update();
    }

    public void FixedUpdate()
    {
        _state.FixedUpdate();
    }

    public void OnTriggerEnter(Collider other)
    {
        _state.OnTriggerEnter(other);
    }

    public void ChangeState(PlayerStates state)
    {
        if (_state != null)
        {
            _state.Dispose();
            _state = null;
        }

        _state = _stateFactory.CreateState(state);
        _state.Start();
    }
}
