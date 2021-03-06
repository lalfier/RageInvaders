using System;
using UnityEngine;

public abstract class PlayerState : IDisposable
{
    public abstract void Update();

    public abstract void FixedUpdate();

    public virtual void Start()
    {
        // optionally overridden
    }

    public virtual void Dispose()
    {
        // optionally overridden
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        // optionally overridden
    }

    public virtual void PlayerHit()
    {
        // optionally overridden
    }
}
