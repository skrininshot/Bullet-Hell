using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CursorController : IInitializable, IDisposable
{
    private readonly List<object> _users = new();

    public void Initialize()
    {
        HandleCursor();
    }

    public void Dispose()
    {
        _users.Clear();

        HandleCursor();
    }

    public void RegisterUser(object user)
    {
        if (!_users.Contains(user))
        {
            _users.Add(user);

            HandleCursor();
        }     
    }

    public void UnregisterUser(object user)
    {
        if (_users.Contains(user))
        {
            _users.Remove(user);

            HandleCursor();
        }        
    }

    private void HandleCursor()
    {
        bool visible = (_users.Count > 0);

        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
