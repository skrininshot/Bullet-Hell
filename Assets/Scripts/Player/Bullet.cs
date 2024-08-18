using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public UnityEvent<bool> OnDestroy = new();
    public Transform CameraPoint => _cameraPoint;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _cameraPoint;

    private float _destroyTime;
    private float _speed;

    [Inject]
    private void Construct(GameSettings gameSettings)
    {
        _destroyTime = gameSettings.Bullet.DestroyTime;
        _speed = gameSettings.Bullet.Speed;
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyTimer(_destroyTime)); 
    }

    private void Update()
    {
        _rigidbody.velocity = _speed * transform.forward;
    }

    private void OnCollisionEnter(Collision collision)
    {
        SelfDestroy(true);
    }

    private IEnumerator DestroyTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SelfDestroy(false);
    }

    private void SelfDestroy(bool collision)
    {
        Destroy(gameObject);
        OnDestroy?.Invoke(collision);
    }

    [Serializable]
    public class Settings
    {
        public float DestroyTime = 5f;
        public float Speed = 5f;
    }

    public class Factory : PlaceholderFactory<Bullet> { }
}
