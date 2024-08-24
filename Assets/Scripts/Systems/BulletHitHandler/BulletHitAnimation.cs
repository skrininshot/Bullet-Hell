using System;

public abstract class BulletHitAnimation : IDisposable
{
    public abstract void Play();

    public virtual void Dispose()
    {

    }
}
