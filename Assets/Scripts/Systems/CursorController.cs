using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CursorController : IInitializable, IDisposable
{
    private List<object> _subscribers = new ();

    public void Initialize()
    {
        HandleCursor();
    }

    public void Dispose()
    {
        HandleCursor();
    }

    public void AddSubscriber(object subscriber)
    {
        _subscribers.Add(subscriber);
        HandleCursor();
    }

    public void RemoveSubscriber(object subscriber)
    {
        _subscribers.Remove(subscriber);
        HandleCursor();
    }

    private void HandleCursor()
    {
        bool visible = (_subscribers.Count > 0);

        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
