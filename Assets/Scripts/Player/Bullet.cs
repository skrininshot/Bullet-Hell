using System;
using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour
{
    public Transform Transform => _transform;

    [HideInInspector] public Action<Collider> OnHit;
    public Transform CameraPoint => _cameraPoint;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _cameraPoint;

    private GameSettings.BulletSettings _settings;
    private AirFlowEffectSpawner _airFlowSpawner;

    private Transform _transform;

    private float _speed;

    [Inject]
    private void Construct(GameSettings gameSettings, 
        AirFlowEffectSpawner airFlowSpawner)
    {
        _settings = gameSettings.Bullet;
        _airFlowSpawner = airFlowSpawner;
    }

    private void Awake()
    {
        _transform = transform;
    }

    private void OnEnable()
    {
        _speed = _settings.Speed;
        _airFlowSpawner.Start();
    }

    private void OnDisable()
    {
        _airFlowSpawner.Stop();
    }

    private void Update()
    {
        _rigidbody.velocity = _speed * _transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnHit?.Invoke(other);
    }
}
