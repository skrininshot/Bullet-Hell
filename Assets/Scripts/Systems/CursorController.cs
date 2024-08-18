using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CursorController : IInitializable, IDisposable
{
    private List<int> _subscribers = new ();

    public void Initialize()
    {
        HandleCursor();
    }

    public void Dispose()
    {
        HandleCursor();
    }

    public void AddSubscriber(int hashCode)
    {
        _subscribers.Add(hashCode);
        HandleCursor();
    }

    public void RemoveSubscriber(int hashCode)
    {
        _subscribers.Remove(hashCode);
        HandleCursor();
    }

    private void HandleCursor()
    {
        bool visible = (_subscribers.Count > 0);

        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
